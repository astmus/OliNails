using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;

namespace MainSite
{
	public partial class OwnControl : System.Web.UI.Page
	{
		NailScheduler scheduler;
		protected void Page_Load(object sender, EventArgs e)
		{
			Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;			
			scheduler = new NailScheduler(Settings.Instance.AvailableTimes, DataBaseHandler.Instance.GetNailDatesFromBeginningWeek(), Mode.Owner);
			scheduler.NailDateSelected += OnNailDateSeleted;
			scheduler.ReservDate += OnReservDatePressed;
			mainPanel.Controls.Add(scheduler);			
		}

		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);
			if (Session["ownChecks"] != null)
				AddServicesCheckBoxToPage(Session["ownChecks"] as List<CheckBox>);
		}

		private void OnReservDatePressed(DateTime obj)
		{
			detailDataTable.Visible = false;			
			DataBaseHandler.Instance.InsertNailDate(obj, TimeSpan.Zero, "Резерв", "",new List<int>());
			Response.Redirect(Request.RawUrl);
		}

		private void AddServicesCheckBoxToPage(List<CheckBox> checks)
		{			
			foreach (var check in checks)
			{
				var row = new TableRow();
				var cell = new TableCell();
				cell.ColumnSpan = 2;
				cell.Controls.Add(check);
				row.Cells.Add(cell);
				detailDataTable.Rows.AddAt(detailDataTable.Rows.Count - 2, row);
			}
			detailDataTable.Visible = true;
		}

		private void OnNailDateSeleted(NailDate obj)
		{
			if (Session["ownChecks"] != null)
			{
				var cks = Session["ownChecks"] as List<CheckBox>;
				foreach (var check in cks)
					detailDataTable.Rows.RemoveAt(detailDataTable.Rows.GetRowIndex(servicesRow)+1);
			}
			
			dateCalendar.SelectedDate = obj.StartTime;
			dateCalendar.DataBind();
			seletedDate.Text = obj.StartTime.ToString();
			clientName.Text = obj.ClientName;
			clientPhone.Text = obj.ClientPhone;

			var services = DataBaseHandler.Instance.GetAvailableServices(); 
			var selectedServices = DataBaseHandler.Instance.GetSelectedServicesForDate(obj.ID);
			services = services.Union(selectedServices).Distinct(new NailServiceComparer()).OrderBy(ob=>ob.Name).ToList();
			
			var checks = GenerateServicesCheckBox(services, selectedServices);
			AddServicesCheckBoxToPage(checks);

			Session["ownChecks"] = checks;
			Session["selectedServices"] = selectedServices;
			Session["selectedNailDate"] = obj;
		}

		List<CheckBox> GenerateServicesCheckBox(List<NailService> availableServices, List<NailService> selectedServices)
		{
			var checks = new List<CheckBox>();
			foreach (var service in availableServices)
			{
				CheckBox box = new CheckBox();
				box.ID = service.ID.ToString();
				box.Text = service.Name;
				box.Checked = selectedServices.FirstOrDefault(a => a.ID == service.ID) != null;
				checks.Add(box);
			}
			return checks;
		}

		protected void DateSelectionChanged(object sender, EventArgs e)
		{
			var calendar = sender as Calendar;
			DateTime d = calendar.SelectedDate;
			var res = DataBaseHandler.Instance.GetAvailableTimesForDate(d);
			availableTimes.DataSource = res;
			availableTimes.DataBind();
		}

		protected void availableTimes_SelectedIndexChanged(object sender, EventArgs e)
		{
			var listBox = sender as ListBox;			
			TimeSpan time = TimeSpan.Parse((string)listBox.SelectedItem.Value);
			seletedDate.Text = dateCalendar.SelectedDate.Add(time).ToString();
		}

		private void HandleNailDateInSessionAndRefresh(Action<NailDate> handler)
		{
			NailDate date = Session["selectedNailDate"] as NailDate;
			Session["selectedNailDate"] = null;
			if (date == null) return;
			handler(date);
			Session.Clear();
			Response.Redirect(Request.RawUrl);
		}

		protected void OnUpdateNialDateClick(object sender, EventArgs e)
		{
			NailDate date = Session["selectedNailDate"] as NailDate;
			date.ClientName = clientName.Text;
			date.ClientPhone = clientPhone.Text;
			date.StartTime = DateTime.Parse(seletedDate.Text);
			var oldServices = (Session["selectedServices"] as List<NailService>).Select(s=>s.ID).ToList();
			var servicesIDs = (Session["ownChecks"] as List<CheckBox>).Where(w => w.Checked == true).Select(s => Convert.ToInt32(s.ID)).ToList();
			HandleNailDateInSessionAndRefresh(nd => DataBaseHandler.Instance.UpdateNailDate(nd, servicesIDs, oldServices));
			Session.Clear();			
		}

		protected void OnDeleteNailDateClick(object sender, EventArgs e)
		{
			HandleNailDateInSessionAndRefresh(nd => DataBaseHandler.Instance.DropNailDate(nd));			
		}
	}

	class NailServiceComparer : IEqualityComparer<NailService>
	{

		public bool Equals(NailService x, NailService y)
		{
			return x.ID == y.ID;
		}

		public int GetHashCode(NailService obj)
		{
			return obj.ID.GetHashCode();
		}
	}
}