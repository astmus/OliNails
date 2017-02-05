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
	public class NailDateCalendar : Calendar
	{
		//private const int DEGREES_IN_HOUR = 30; for clock mode
		//private const int DEGREES_IN_MINUTE = 6; for clock mode
		public TimeSpan WorkDayStartAt = TimeSpan.FromHours(10);
		public TimeSpan WorkDayLength = TimeSpan.FromHours(10);
		public List<TimeSpan> AwailableTimes { get; set; }

		private DateTime? _lastDate;
		private string _controlId = Guid.NewGuid().ToString();

		protected override ControlCollection CreateControlCollection()
		{
			return base.CreateControlCollection();
		}

		protected override void OnSelectionChanged()
		{
			base.OnSelectionChanged();
		}

		protected override void OnDayRender(TableCell cell, CalendarDay day)
		{
			if (day.Date < DateTime.Now.Date) // if cell date less that now we just block cell for not to do unnecessary actions 
			{
				cell.Text = "";
				base.OnDayRender(cell, day);
				return;
			}

			if (!_lastDate.HasValue)
			{
				_lastDate = day.Date.AddDays(42);
				var now = DateTime.Now.Date;
				DateTime? forDate = _forDate;
				if (!forDate.HasValue || forDate != day.Date)
					LoadNailDatesForRange(day.Date, _lastDate.Value);
			}

			if (day.Date == _lastDate)
				_lastDate = null;


			var dates = _nailDates.Where(w => w.StartTime.Date == day.Date).ToArray();
			switch (dates.Length)
			{
				case 3: // then all day is already reserved, disable reserve functionality
					cell.Text = "";
					break;
				case 0: // all is available for reserve, do nothing for display reserves
					break;
				default: // one or two times reserved and we must display it on the background
					string backgroundImage = GenerateTimedGradient(dates);
					cell.Style.Add("background", backgroundImage);
					break;
			}
			base.OnDayRender(cell, day);
		}

		private void LoadNailDatesForRange(DateTime start, DateTime end)
		{
			if (start < DateTime.Now.Date) // we must load only future nail dates
				start = DateTime.Now.Date;
			if (start >= end)
				_forDate = start;
			else
			{
				_nailDates = DataBaseHandler.Instance.GetNailDatesForTimeRange(start, end);
				_forDate = start;
			}
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

		private List<NailDate> _nailDates
		{
			get
			{
				return (List<NailDate>)HttpContext.Current.Session[_controlId + "nailDates"] ?? new List<NailDate>();
			}
			set
			{
				HttpContext.Current.Session[_controlId + "nailDates"] = value;
			}
		}

		private DateTime? _forDate
		{
			get
			{
				return (DateTime?)HttpContext.Current.Session[_controlId + "forDate"];
			}
			set
			{
				HttpContext.Current.Session[_controlId + "forDate"] = value;
			}
		}

		private int TimeToPercent(TimeSpan s)
		{
			double startMin = s.TotalMinutes - WorkDayStartAt.TotalMinutes;
			double oneMinPerc = 100 / WorkDayLength.TotalMinutes;
			return (int)(startMin * oneMinPerc);
		}
	}
}
