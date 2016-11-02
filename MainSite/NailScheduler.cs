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
		private Mode _currentMode { get; set; }
		private List<string> _timeList;
		public delegate void AddDateRecordCallback(DateTime startTime);
		public event AddDateRecordCallback CreateNailDate;
		public event Action<NailDate> NailDateSelected;
		public event Action<DateTime> ReservDate;
		public NailScheduler(List<string> timeList, List<NailDate> nailDates, Mode workForMode)
		{
			CreateHeader();			
			_timeList = timeList;
			_currentMode = workForMode;
			weeks = new List<WorkWeek>();
			CellSpacing = 1;
			CellPadding = 0;
			//BackColor = Color.Black;
			BorderColor = Color.Black;
			BorderStyle = BorderStyle.Solid;
			BorderWidth = 2;
			
			var startDay = DateTimeHelper.getStartOfCurrentWeek();
			for (int i = 0; i < CountOfNextWeeks; i++)
			{
				var row = new TableRow();				
				var dateCell = new TableCell();
				var endDay = startDay.AddDays(7);
				var week = new WorkWeek(startDay, nailDates.Where(w=>w.StartTime.Date >= startDay.Date && w.StartTime.Date <= endDay.Date).ToList(), _currentMode);

				switch (_currentMode)
				{
					case Mode.User:
						week.AddNewDateButtonPressed = OnAddNewDateButtonPressed;
					break;
					case Mode.Owner:
						week.NailDateSelected = OnNailDateSelected;
						week.ReservDatePressed = OnReservDatePressed;
					break;
				}
								
				dateCell.Controls.Add(week);								
				row.Cells.Add(dateCell);
				row.BorderColor = Color.Gray;
				Rows.Add(row);
				startDay = endDay;
			}		
		}

		private void OnReservDatePressed(DateTime certainTime)
		{
			ReservDate?.Invoke(certainTime);
		}

		private void OnNailDateSelected(NailDate date)
		{
			NailDateSelected?.Invoke(date);
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

		
	}
}
