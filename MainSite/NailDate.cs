using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MainSite
{
	public class NailDate
	{
		public DateTime StartTime { get; set; }
		public TimeSpan Duration { get; set; }
		public String ClientPhone { get; set; }
		public String ClientName { get; set; }
		public int ID { get; set; }
		public short? Tips { get; set; }
		public NailDate()
		{

		}

		public static NailDate Parse(SqlDataReader reader)
		{
			var result = new NailDate();
			result.ID = (int)reader["id"];
			result.StartTime = (DateTime)reader["StartTime"];
#warning fix this dummy when need real duration of nail date
			result.Duration = (result.StartTime.Hour != 13) ? TimeSpan.FromHours(3): TimeSpan.FromHours(4); //TimeSpan.FromTicks((Int64)reader["Duration"]);
			result.ClientName = reader["ClientName"] as string;
			result.ClientPhone = reader["ClientPhone"] as string;
			result.Tips = reader["tips"] != System.DBNull.Value ? (short)reader["tips"] : (short?)null;
			return result;
		}
	}
}