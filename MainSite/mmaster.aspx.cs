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

		protected void scheduler_SelectionChanged(object sender, EventArgs e)
		{
			int i = 0;
		}		
	}
}