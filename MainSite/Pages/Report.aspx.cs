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
		protected void Page_Load(object sender, EventArgs e)
		{
			if (dateFrom.SelectedDate == DateTime.MinValue && dateTo.SelectedDate == DateTime.MinValue)
			{
				var currDate = DateTimeHelper.currentLocalDateTime();
				dateFrom.TodaysDate = currDate.Date.AddDays((currDate.Day - 1) * -1);
				dateFrom.SelectedDate = dateFrom.TodaysDate;
				dateTo.TodaysDate = currDate;
				dateTo.SelectedDate = dateTo.TodaysDate;				
			}
			dateTo.SelectionChanged += OnDateToChanged;
		}

		private void OnDateToChanged(object sender, EventArgs e)
		{
			GridView1.DataBind();
		}

		protected void NailDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
		{
			e.Command.Parameters["@StartTime"].Value = DateTimeHelper.currentLocalDateTime();
			e.Command.Parameters["@from"].Value = dateFrom.SelectedDate;
			e.Command.Parameters["@to"].Value = dateTo.SelectedDate;
		}

		protected void GridView1_DataBound(object sender, EventArgs e)
		{
			GridView1.FooterRow.Cells[0].Text = "Суммарно";
			GridView1.FooterRow.Cells[1].Text = String.Format("{0} часов", totalTime / 60);
			GridView1.FooterRow.Cells[5].Text = String.Format("{0} руб.", totalPrice);
		}

		protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				int currPrice = 0;
				int currTime = 0;
				if (int.TryParse(e.Row.Cells[1].Text, out currTime))
					totalTime += currTime;

				if (int.TryParse(e.Row.Cells[5].Text, out currPrice))
					totalPrice += currPrice;
			}
		}
	}
}