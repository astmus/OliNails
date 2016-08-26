using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite
{
	[ToolboxData("<{0}:WebCustomControl1 runat=server></{0}:WebCustomControl1>")]
	public class NailScheduler : Table
	{
		public List<WorkWeek> weeks { get; set; }
		public int CountOfNextWeeks { get; set; } = 5;

		private List<string> _timeList;
		public delegate void AddDateRecordCallback(DateTime startTime);
		public event AddDateRecordCallback CreateNailDate;

		public NailScheduler(List<string> timeList, List<NailDate> nailDates)
		{
			CreateHeader();			
			_timeList = timeList;
			weeks = new List<WorkWeek>();
			CellSpacing = 1;
			BorderWidth = 1;
			BorderColor = Color.Gray;

			var startDay = getStartOfCurrentWeek();
			for (int i = 0; i < CountOfNextWeeks; i++)
			{
				var row = new TableRow();
				var dateCell = new TableCell();
				var endDay = startDay.AddDays(7);
				var week = new WorkWeek(startDay, nailDates.Where(w=>w.StartTime.Date >= startDay.Date && w.StartTime.Date <= endDay.Date).ToList());
				week.AddNewDateButtonPressed += OnAddNewDateButtonPressed;
				dateCell.Controls.Add(week);				
				row.Cells.Add(dateCell);
				row.BorderColor = Color.Gray;
				Rows.Add(row);
				startDay = endDay;
			}		
		}

		private void OnAddNewDateButtonPressed(DateTime obj)
		{
			CreateNailDate?.Invoke(obj);
		}

		private void CreateHeader()
		{
			TableHeaderRow mainTitle = new TableHeaderRow();
			mainTitle.BackColor = Color.Gray;
			TableHeaderCell cell = new TableHeaderCell();
			cell.ColumnSpan = 8;			
			cell.Text = "Расписаине";
			mainTitle.Cells.Add(cell);
			Rows.Add(mainTitle);
		}

		public static DateTime getStartOfCurrentWeek()
		{
			DateTime nowDateTime = DateTime.UtcNow;
			DateTime newDateTime = TimeZoneInfo.ConvertTime(
				nowDateTime,
				TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
			return newDateTime.AddDays(-1 * (int)(newDateTime.DayOfWeek - 1));
		}
	}
}
