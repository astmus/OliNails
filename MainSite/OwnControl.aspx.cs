using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.Services;
using System.IO;

namespace MainSite
{
	public partial class OwnControl : System.Web.UI.Page
	{
#region WebMethods

		[WebMethod]
		public static string GetNoteMessage(int date)
		{
			DateTime dateTime = new DateTime(TimeSpan.FromDays(date).Ticks);
			if (System.Web.HttpContext.Current.Session["date"] == null)
				System.Web.HttpContext.Current.Session.Add("date", dateTime);
			else
				System.Web.HttpContext.Current.Session["date"] = dateTime;
			
			return DataBaseHandler.Instance.GetNote(dateTime)?.note;
		}

		[WebMethod]
		public static void SaveNote(int date, string message)
		{
			DataBaseHandler.Instance.SaveNote(new DateTime(TimeSpan.FromDays(date).Ticks), message);
		}

		[WebMethod]
		public static void DeleteNote(int date)
		{
			DataBaseHandler.Instance.DeleteNote(new DateTime(TimeSpan.FromDays(date).Ticks));
		}
		#endregion

		NailScheduler scheduler;
		protected void Page_Load(object sender, EventArgs e)
		{
			Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
			int daysShift = int.Parse(Request.Params["addDays"] ?? "0");
			scheduler = new NailScheduler(Settings.Instance.AvailableTimes, DateTimeHelper.getStartOfCurrentWeek().Date.AddDays(daysShift), Mode.Owner);
			scheduler.NailDateSelected += OnNailDateSeleted;
			scheduler.ReservDate += OnReservDatePressed;
			mainPanel.Controls.Add(scheduler);
			
			//LogError("page loaded " + DateTime.Now.ToShortDateString()+" "+ DateTime.Now.ToShortTimeString());
			CreateFile();
		}

		private void CreateFile()
		{
			string folderPath = Server.MapPath("~/Logs/");
			
			string userFilename = Path.Combine(folderPath, DateTime.Now.ToString("mm-ss") + ".txt");

			using (FileStream stream = System.IO.File.Create(userFilename))
			{
				StreamWriter strwr = new StreamWriter(stream);
				strwr.WriteLine("new file");
				strwr.Close();
				stream.Close();
			}			
		}

		private void LogError(string message)
		{
			string path = Server.MapPath("~/Logs/log.txt");
			using (StreamWriter writer = new StreamWriter(path, true))
			{
				writer.WriteLine(message);
				writer.Close();
			}
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
			tipsField.Text = obj.Tips.ToString();

			var services = DataBaseHandler.Instance.GetAvailableServices(); 
			var selectedServices = DataBaseHandler.Instance.GetSelectedServicesForDate(obj.ID);
			services = services.Union(selectedServices).Distinct(new NailServiceComparer()).ToList();
			
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
			short tmpTips = 0;
			if (short.TryParse(tipsField.Text,out tmpTips))
			{
				date.Tips = tmpTips;
			}			
			var oldServices = (Session["selectedServices"] as List<NailService>).Select(s=>s.ID).ToList();
			var servicesIDs = (Session["ownChecks"] as List<CheckBox>).Where(w => w.Checked == true).Select(s => Convert.ToInt32(s.ID)).ToList();
			HandleNailDateInSessionAndRefresh(nd => DataBaseHandler.Instance.UpdateNailDate(nd, servicesIDs, oldServices));
			Session.Clear();			
		}

		protected void OnDeleteNailDateClick(object sender, EventArgs e)
		{
			HandleNailDateInSessionAndRefresh(nd => DataBaseHandler.Instance.DropNailDate(nd));			
		}

		protected void SaveNote(object sender, EventArgs e)
		{			
			DateTime date = (DateTime)Session["date"];
			DataBaseHandler.Instance.SaveNote(date, note.Text);
			Response.Redirect(Request.RawUrl);
			Session.Clear();
		}

		protected void OnDeleteNote_Click(object sender, EventArgs e)
		{
			DateTime date = (DateTime)Session["date"];
			DataBaseHandler.Instance.DeleteNote(date);
			Response.Redirect(Request.RawUrl);
			Session.Clear();
		}

		protected void OnPrevMothClick(object sender, EventArgs e)
		{
			int days = int.Parse(Request.Params["addDays"] ?? "0") - 35;			
			Response.Redirect("OwnControl.aspx?addDays="+days);
		}

		protected void OnNextMonthClick(object sender, EventArgs e)
		{
			int days = int.Parse(Request.Params["addDays"] ?? "0") + 35;
			Response.Redirect("OwnControl.aspx?addDays=" + days);
		}

		protected void OnNextWeekClick(object sender, EventArgs e)
		{
			int days = int.Parse(Request.Params["addDays"] ?? "0") + 7 ;
			Response.Redirect("OwnControl.aspx?addDays=" + days);
		}

		protected void OnPrevWeekClick(object sender, EventArgs e)
		{
			int days = int.Parse(Request.Params["addDays"] ?? "0") - 7;
			Response.Redirect("OwnControl.aspx?addDays=" + days);
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