using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite.Pages
{
	public partial class Report : System.Web.UI.Page
	{
		private int totalPrice = 0;
		private int totalTime = 0;
		private int totalTips = 0;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (dateFrom.SelectedDate == DateTime.MinValue && dateTo.SelectedDate == DateTime.MinValue)
			{
				var currDate = DateTimeHelper.currentLocalDateTime().Date;
				dateFrom.TodaysDate = currDate;
				dateFrom.SelectedDate = dateFrom.TodaysDate;
				dateTo.TodaysDate = currDate;
				dateTo.SelectedDate = dateTo.TodaysDate;				
			}
			dateTo.SelectionChanged += OnDateToChanged;
			dateFrom.SelectionChanged += OnDateToChanged;
		countOfVisitors.Text = Logger.Instance.GetUserCountForCurrentMonth().ToString();
		}

		private void OnDateToChanged(object sender, EventArgs e)
		{
			GridView1.DataBind();
		}

		protected void NailDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
		{		
			e.Command.Parameters["@StartTime"].Value = DateTimeHelper.currentLocalDateTime();
			e.Command.Parameters["@from"].Value = dateFrom.SelectedDate;
			e.Command.Parameters["@to"].Value = dateTo.SelectedDate.AddDays(1);
		}

		protected void GridView1_DataBound(object sender, EventArgs e)
		{
			if (GridView1.FooterRow == null) return;
			GridView1.FooterRow.Cells[1].Text = "Суммарно";
			GridView1.FooterRow.Cells[2].Text = String.Format("{0} часов", totalTime / 60);
			GridView1.FooterRow.Cells[6].Text = String.Format("{0} руб.", totalPrice);
			GridView1.FooterRow.Cells[7].Text = String.Format("{0} руб.", totalTips+totalPrice);
		}

		protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				int currPrice = 0;
				int currTime = 0;
				int currTips = 0;
				if (int.TryParse(e.Row.Cells[2].Text, out currTime))
					totalTime += currTime;

				if (int.TryParse(e.Row.Cells[6].Text, out currPrice))
					totalPrice += currPrice;

				if (int.TryParse(e.Row.Cells[7].Text, out currTips))
					totalTips += currTips;
			}
		}

		protected void OnFindClick(object sender, EventArgs e)
		{
			string param = searchParam.Text;
			NailDataSource.SelectCommand = String.Format("select * from FullNailDatesInfo2 where [StartTime] <= @StartTime and CHARINDEX('{0}',ClientPhone) > 0 or CHARINDEX(N'{0}',ClientName) > 0 or CHARINDEX(N'{0}',procedures) > 0", param);
		}

		protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
		{
			e.Cancel = true;
			string error = null;
			if (e.NewValues[0] == null || e.NewValues[0].ToString().Length < 3)
				error = "слишком короткое имя";
			if (e.NewValues[1] == null || e.NewValues[1].ToString().Length < 10)
				error = "неверный номер телефона";
			if (error != null)
			{
				ShowAlertBox(error);
				return;
			}

			int tips = 0;
			if (int.TryParse(e.NewValues[2] as string, out tips))
				DataBaseHandler.Instance.UpdateReportInfo(int.Parse(e.OldValues[0] as string),e.NewValues[0] as String, e.NewValues[1] as String, tips);
			else
				DataBaseHandler.Instance.UpdateReportInfo(int.Parse(e.OldValues[0] as string),e.NewValues[0] as String, e.NewValues[1] as String, null);
			GridView1.EditIndex = -1;
			GridView1.DataBind();
		}

		public void ShowAlertBox(string message)
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('" + message + "');", true);
		}
	}
}