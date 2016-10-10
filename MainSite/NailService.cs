using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MainSite
{
	public class NailService
	{
		public bool IsObsolete { get; set; }
		public String Abbreviation { get; set; }
		public TimeSpan Duration { get; set; }
		public Int16 Price { get; set; }
		public String Name { get; set; }
		public int ID { get; set; }
		public NailService()
		{

		}

		public static NailService Parse(SqlDataReader reader)
		{
			var result = new NailService();
			result.ID = (int)reader["id"];
			result.Name = reader["name"] as string;
			result.Price = (Int16)reader["price"];
			result.Duration = TimeSpan.FromMinutes((Int16)reader["duration"]);
			result.Abbreviation = reader["abbreviation"] as string;
			result.IsObsolete = (bool)reader["isObsolete"];
			return result;
		}
	}
}