﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MainSite
{
	public class DataBaseHandler
	{
		private static DataBaseHandler _instance;

		private DataBaseHandler() { }
		public static string LastErrorMessage = null;
		public static DataBaseHandler Instance
		{
			get
			{				
				return _instance ?? (_instance = new DataBaseHandler());
			}
		}

		public bool AddNewService(string name, int price, int duration, string abbr)
		{
			//IDENT_CURRENT('NailDates')
			string query = "INSERT INTO Services(name, price, duration, abbreviation) VALUES (@name, @price, @duration, @abbreviation);";
			query += "INSERT INTO ServicePrice(serviceId, startDate, value) VALUES (IDENT_CURRENT('Services'), @startDate, @price);";

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@name", SqlDbType.NVarChar,35).Value = name;
				cmd.Parameters.Add("@price", SqlDbType.SmallInt).Value = price;
				cmd.Parameters.Add("@duration", SqlDbType.SmallInt).Value = duration;
				cmd.Parameters.Add("@abbreviation", SqlDbType.NVarChar,3).Value = abbr;
				cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = DateTimeHelper.currentLocalDateTime().Date;
				cn.Open();
				try
				{
					var res = cmd.ExecuteNonQuery();
					return true;
				}
				catch (System.Exception ex)
				{
					LastErrorMessage = ex.Message;
					return false;
				}
				finally
				{
					cn.Close();
				}				
			}			
		}

		public List<NailService> GetSelectedServicesForDate(int nailDateID)
		{			
			string query = "select * from Services where Services.id in (SELECT serviceId FROM dbo.NailDateService where nailDateId = @ID)";
			var services = new List<NailService>();

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@ID", SqlDbType.Int).Value = nailDateID;
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
					services.Add(NailService.Parse(dr));
				cn.Close();
			}
			return services;
		}

		public List<NailService> GetAvailableServices()
		{
			string query = "select * from Services where isObsolete = 'False'";
			var services = new List<NailService>();

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{				
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
					services.Add(NailService.Parse(dr));
				cn.Close();
			}
			return services;
		}

		public List<NailDate> GetFutureNailDates()
		{
			string query = "select * from NailDates where StartTime >= @DateFrom";
			var dates = new List<NailDate>();
			// create connection and command
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = DateTimeHelper.getStartOfCurrentWeek();
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
					dates.Add(NailDate.Parse(dr));
				cn.Close();
			}
			return dates;
		}

		public void DropNailDate(NailDate date)
		{
			string query = "delete from dbo.NailDates where Id = @ID;";
			query += "delete from dbo.NailDateService where nailDateId = @ID;";

			// create connection and command
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				// define parameters and their values
				cmd.Parameters.Add("@ID", SqlDbType.Int).Value = date.ID;				
				cn.Open();
				cmd.ExecuteNonQuery();
				cn.Close();
			}
		}

		public List<string> GetAvailableTimesForDate(DateTime date)
		{
			string query = "select StartTime from NailDates where StartTime >= @DateFrom and StartTime < @DateTo";
			var reservedDates = new List<string>();
			// create connection and command
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = date.Date;
				cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = date.Date.AddDays(1);
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
					reservedDates.Add(((DateTime)dr["StartTime"]).ToString("HH:mm"));
				cn.Close();
			}

			return Settings.Instance.AvailableTimes.Except(reservedDates).ToList(); 
		}

		public void UpdateNailDate(NailDate date,List<int> currentSelServicesIDs, List<int> oldServicesIDs)
		{
			string query = "update dbo.NailDates set StartTime = @StartTime, Duration = @Duration, ClientName = @ClientName, ClientPhone = @ClientPhone where id=@ID;";
			if (currentSelServicesIDs.Count == 0)
				query += "delete from dbo.NailDateService where nailDateId = @ID;";
			else
				query += "delete from dbo.NailDateService where nailDateId = @ID and serviceId not in ("+String.Join(",",currentSelServicesIDs) +");";

			var needToAddServices = currentSelServicesIDs.Except(oldServicesIDs).ToList();			
			needToAddServices.ForEach(fe => query += String.Format("insert into dbo.NailDateService (nailDateId, serviceId) values (@ID, @serviceId{0});", fe));

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@ID", SqlDbType.Int).Value = date.ID;
				cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = date.StartTime;
				cmd.Parameters.Add("@Duration", SqlDbType.BigInt).Value = date.Duration.Ticks;
				cmd.Parameters.Add("@ClientName", SqlDbType.NText, 20).Value = date.ClientName;
				cmd.Parameters.Add("@ClientPhone", SqlDbType.VarChar, 15).Value = date.ClientPhone;
				needToAddServices.ForEach(i=> cmd.Parameters.Add(String.Format("@serviceId{0}", i), SqlDbType.Int).Value = i);
				cn.Open();
				cmd.ExecuteNonQuery();
				cn.Close();
			}
		}

		public void InsertNailDate(DateTime startDate, TimeSpan duration, string userName, string userPhone, List<int> listOFServicesIDs)
		{
			// define INSERT query with parameters
			string query = "INSERT INTO dbo.NailDates (StartTime, Duration, ClientName, ClientPhone) " +
						   "VALUES (@StartTime, @Duration, @ClientName, @ClientPhone); ";
			uint pos = 0;
			foreach (var serviceId in listOFServicesIDs)			
				query += String.Format("insert into dbo.NailDateService (nailDateId, serviceId) values (IDENT_CURRENT('NailDates'), @serviceId{0});",pos++);
			
			// create connection and command
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				// define parameters and their values
				cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = startDate;
				cmd.Parameters.Add("@Duration", SqlDbType.BigInt).Value = duration.Ticks;
				cmd.Parameters.Add("@ClientName", SqlDbType.NText, 20).Value = userName;
				cmd.Parameters.Add("@ClientPhone", SqlDbType.VarChar, 15).Value = userPhone;
				for (int i = 0; i < pos; i++)
					cmd.Parameters.Add(String.Format("@serviceId{0}",i), SqlDbType.Int).Value = listOFServicesIDs[i];

				// open connection, execute INSERT, close connection
				cn.Open();
				cmd.ExecuteNonQuery();
				cn.Close();
			}
		}
	}
}