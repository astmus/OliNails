using System;
using System.Data.SqlClient;

namespace MainSite
{
	public class NailDate
	{
		public DateTime StartTime { get; set; }
		public TimeSpan Duration { get; set; }
		public String ClientPhone { get; set; }
		public String ClientName { get; set; }

		public NailDate()
		{

		}

		public static NailDate Parse(SqlDataReader reader)
		{
			var result = new NailDate();
			result.StartTime = (DateTime)reader["StartTime"];
			result.Duration = TimeSpan.FromTicks((Int64)reader["Duration"]);
			result.ClientName = reader["ClientName"] as string;
			result.ClientPhone = reader["ClientPhone"] as string;
			return result;
		}
	}
}