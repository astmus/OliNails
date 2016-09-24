using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MainSite
{
	public class DataBaseHandler
	{
		private static DataBaseHandler _instance;

		private DataBaseHandler() { }

		public static DataBaseHandler Instance
		{
			get
			{				
				return _instance ?? (_instance = new DataBaseHandler());
			}
		}

		public List<NailDate> GetFutureNailDates()
		{
			string query = "select * from NailDates where StartTime >= @DateFrom";
			var dates = new List<NailDate>();
			// create connection and command
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = NailScheduler.getStartOfCurrentWeek();
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
					dates.Add(NailDate.Parse(dr));
				cn.Close();
			}
			return dates;
		}

		public void InsertNailDate(DateTime startDate, TimeSpan duration, string userName, string userPhone)
		{
			// define INSERT query with parameters
			string query = "INSERT INTO dbo.NailDates (StartTime, Duration, ClientName, ClientPhone) " +
						   "VALUES (@StartTime, @Duration, @ClientName, @ClientPhone) ";

			// create connection and command
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				// define parameters and their values
				cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = startDate;
				cmd.Parameters.Add("@Duration", SqlDbType.BigInt).Value = duration.Ticks;
				cmd.Parameters.Add("@ClientName", SqlDbType.NText, 20).Value = userName;
				cmd.Parameters.Add("@ClientPhone", SqlDbType.VarChar, 15).Value = userPhone;

				// open connection, execute INSERT, close connection
				cn.Open();
				cmd.ExecuteNonQuery();
				cn.Close();
			}
		}
	}
}