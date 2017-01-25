using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainSite
{
	public static class DateTimeHelper
	{
		public static DateTime getStartOfCurrentWeek()
		{
			DateTime nowDateTime = DateTime.UtcNow;			
			return nowDateTime.AddDays(1 - (nowDateTime.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)nowDateTime.DayOfWeek));
		}		
	}
}