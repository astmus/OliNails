using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace MainSite.Controls
{	
	[ToolboxData("<{0}:NailDateCalendar runat=server></{0}:NailDateCalendar>")]
	public class NailDateCalendar : Calendar // 9 hours o'clock is equal to 0 degrees 
	{
		//private const int DEGREES_IN_HOUR = 30; for clock mode
		//private const int DEGREES_IN_MINUTE = 6; for clock mode
		public TimeSpan WorkDayStartAt = TimeSpan.FromHours(10);
		public TimeSpan WorkDayLength = TimeSpan.FromHours(10);
		public List<TimeSpan> AwailableTimes { get; set; }
		protected override void OnSelectionChanged()
		{
			base.OnSelectionChanged();
		}

		static TimeSpan t = TimeSpan.FromHours(9);
		protected override void OnDayRender(TableCell cell, CalendarDay day)
		{
			if (!day.IsSelected)
			{
				NailDate d = new NailDate() { StartTime = DateTime.Now.Date.AddHours(10), Duration=TimeSpan.FromHours(3) };
				NailDate d2 = new NailDate() { StartTime = DateTime.Now.Date.AddHours(13), Duration = TimeSpan.FromHours(4) };
				NailDate d3 = new NailDate() { StartTime = DateTime.Now.Date.AddHours(17), Duration = TimeSpan.FromHours(3) };
				string backgroundImage = GenerateTimedGradient(d,d2,d3);
				cell.Style.Add("background", backgroundImage);
				t = t.Add(TimeSpan.FromHours(1));
			}
			base.OnDayRender(cell, day);
		}	

		protected override void RenderContents(HtmlTextWriter output)
		{			
		}

		private string GenerateTimedGradient(params NailDate[] dates)
		{
			int val = 100 / AwailableTimes.Count;
			StringBuilder b = new StringBuilder("linear-gradient(");
			foreach (var date in dates)
			{
				int start = TimeToPercent(date.StartTime.TimeOfDay);
				int end = TimeToPercent(date.StartTime.Add(date.Duration).TimeOfDay);
				b.AppendFormat("transparent {0}%,#bd82fa {0}%, #bd82fa {1}%, transparent {1}%,", start, end);
			}
			b.Length = b.Length - 1;
			b.Append(")");			
			return b.ToString();			
		}

		private int TimeToPercent(TimeSpan s)
		{
			double startMin = s.TotalMinutes - WorkDayStartAt.TotalMinutes;
			double oneMinPerc = 100 / WorkDayLength.TotalMinutes;
			return (int)(startMin * oneMinPerc);
		}
	}
}
