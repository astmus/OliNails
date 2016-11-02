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
			DateTime newDateTime = TimeZoneInfo.ConvertTime(
				nowDateTime,
				TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
			return newDateTime.AddDays(1 - (newDateTime.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)newDateTime.DayOfWeek));
		}

		public static DateTime currentLocalDateTime()
		{
			DateTime nowDateTime = DateTime.UtcNow;
			return TimeZoneInfo.ConvertTime(
				nowDateTime,
				TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
		}
	}
}