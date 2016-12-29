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
				var currDate = DateTimeHelper.currentLocalDateTime();
				dateFrom.TodaysDate = currDate.Date.AddDays((currDate.Day - 1) * -1);
				dateFrom.SelectedDate = dateFrom.TodaysDate;
				dateTo.TodaysDate = currDate;
				dateTo.SelectedDate = dateTo.TodaysDate;				
			}
			dateTo.SelectionChanged += OnDateToChanged;
			countOfVisitors.Text = Application["countOfVisitors"].ToString();
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
			GridView1.FooterRow.Cells[0].Text = "Суммарно";
			GridView1.FooterRow.Cells[1].Text = String.Format("{0} часов", totalTime / 60);
			GridView1.FooterRow.Cells[5].Text = String.Format("{0} руб.", totalPrice);
			GridView1.FooterRow.Cells[6].Text = String.Format("{0} руб.", totalTips);
		}

		protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				int currPrice = 0;
				int currTime = 0;
				int currTips = 0;
				if (int.TryParse(e.Row.Cells[1].Text, out currTime))
					totalTime += currTime;

				if (int.TryParse(e.Row.Cells[5].Text, out currPrice))
					totalPrice += currPrice;

				if (int.TryParse(e.Row.Cells[6].Text, out currTips))
					totalTips += currTips;
			}
		}

		protected void OnFindClick(object sender, EventArgs e)
		{
			string param = searchParam.Text;
			NailDataSource.SelectCommand = String.Format("select * from FullNailDatesInfo2 where [StartTime] <= @StartTime and CHARINDEX('{0}',ClientPhone) > 0 or CHARINDEX(N'{0}',ClientName) > 0 or CHARINDEX(N'{0}',procedures) > 0", param);
		}
	}
}