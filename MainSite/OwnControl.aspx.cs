using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite
{
	public partial class OwnControl : System.Web.UI.Page
	{
		NailScheduler scheduler;
		protected void Page_Load(object sender, EventArgs e)
		{
			Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;			
			scheduler = new NailScheduler(Settings.Instance.AvailableTimes, DataBaseHandler.Instance.GetFutureNailDates(), Mode.Owner);
			scheduler.NailDateSelected += OnNailDateSeleted;
			scheduler.ReservDate += OnReservDatePressed;
			mainPanel.Controls.Add(scheduler);			
		}

		private void OnReservDatePressed(DateTime obj)
		{
			detailDataTable.Visible = false;
		}

		private void OnNailDateSeleted(NailDate obj)
		{
			dateCalendar.SelectedDate = obj.StartTime;			
			seletedDate.Text = obj.StartTime.ToString();
			clientName.Text = obj.ClientName;
			clientPhone.Text = obj.ClientPhone;
			detailDataTable.Visible = true;
			Session["selectedNailDate"] = obj;
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

		protected void OnUpdateNialDateClick(object sender, EventArgs e)
		{

		}

		protected void OnDeleteNailDateClick(object sender, EventArgs e)
		{
			NailDate date = Session["selectedNailDate"] as NailDate;
			Session["selectedNailDate"] = null;
			if (date == null) return;
			DataBaseHandler.Instance.DropNailDate(date);
			Response.Redirect(Request.RawUrl);
		}
	}
}