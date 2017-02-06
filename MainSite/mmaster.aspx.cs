using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite
{
	public partial class mmaster : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			scheduler.AwailableTimes = Settings.Instance.AvailableTimes.Select(s => TimeSpan.Parse(s)).ToList();
			SelectedDatesCollection dates = scheduler.SelectedDates;
		}

		protected void scheduler_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
		{

		}
				
		protected void scheduler_SelectionChanged1(List<NailDate> obj)
		{			
			buttonsPanel.Visible = true;
			foreach (string time in Settings.Instance.AvailableTimes)
			{
				TimeSpan span = TimeSpan.Parse(time);
				Button b = new Button();
				if (obj.Any(a => a.StartTime.TimeOfDay == span))
				{
					b.Text = "Занято/Изменить";
					b.CssClass = "reserved";
				}
				else
				{
					b.Text = "Записаться на " + time;
					b.CssClass = "notreserved";
				}
				buttonsPanel.Controls.Add(b);
			}
		}
	}
}