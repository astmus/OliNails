using System;
using System.Data.SqlClient;

namespace MainSite.DataModels
{
	public class Note
	{
		public int id { get; set; }
		public DateTime date { get; set; }
		public string note { get; set; }

		public static Note FromDataReader(SqlDataReader reader)
		{
			var result = new Note();
			result.id = (int)reader["id"];
			result.date = (DateTime)reader["date"];
			result.note = reader["note"] as string;
			return result;
		}
	}
}