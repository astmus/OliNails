﻿using MainSite.DataModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

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

        public void SaveUsedMaterialsForNailDate(int nailDateId, List<int> materialsIds)
        {
            string query = "delete from dbo.UsedMaterials where nailDateId = @ID;";
            query += "insert into dbo.UsedMaterials(nailDateId, materialId) values " + string.Join(",", materialsIds.Select(s=> String.Format("(@ID, {0})", s)).ToList());
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = nailDateId;
                cn.Open();
                try
                {
                    var res = cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    LastErrorMessage = ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }
        }

		public NailDate GetNailDateById(int nailDateId)
		{
			string query = "select * from NailDates where Id >= @id";
			NailDate dates = null;
			// create connection and command
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@id", SqlDbType.Int).Value = nailDateId;				
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				try
				{
					dr.Read();
					dates = NailDate.Parse(dr);
				}
				catch (System.Exception ex)
				{
					LastErrorMessage = ex.Message;
				}
				finally
				{
					cn.Close();
				}				
			}
			return dates;
		}

		public bool UpdateRelatedToMaterialServices(int materialId, List<string> relatedServicesIDs)
		{
			string query = "delete from dbo.MaterialServices where materialId = @ID;";

			relatedServicesIDs.ForEach(fe => query += String.Format("insert into dbo.MaterialServices (materialId, serviceId) values (@ID, @serviceId{0});", fe));

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@ID", SqlDbType.Int).Value = materialId;
				relatedServicesIDs.ForEach(i => cmd.Parameters.Add(String.Format("@serviceId{0}", i), SqlDbType.Int).Value = i);
				try
				{
					cn.Open();
					cmd.ExecuteNonQuery();
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

		public bool StartNewMaterial(string oldMaterialId)
		{
			string query = "StartNewMaterial";

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query.ToString(), cn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@oldMaterialId",oldMaterialId));
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

		public void UpdateReportInfo(int id, string name, string phone, int? tips, int? realDuration)
		{
			string query = "update dbo.NailDates set ClientName = @name, ClientPhone = @phone, tips = @tips, Duration = @duration where id=@ID;";

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query.ToString(), cn))
			{
				cmd.Parameters.Add("@name", SqlDbType.NText).Value = name;
				cmd.Parameters.Add("@phone", SqlDbType.NVarChar,15).Value = phone;
				cmd.Parameters.Add("@tips", SqlDbType.SmallInt).Value = (object)tips ?? DBNull.Value;
				cmd.Parameters.Add("@duration", SqlDbType.BigInt).Value = (object)realDuration ?? 0;
				cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
				cn.Open();
				try
				{
					var res = cmd.ExecuteNonQuery();
				}
				catch (System.Exception ex)
				{
					LastErrorMessage = ex.Message;
				}
				finally
				{
					cn.Close();
				}
			}
		}

		public void DeleteNote(DateTime forDate)
		{
			string query = "delete from notes where date = @date";

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@date", SqlDbType.Date).Value = forDate.Date;				
				cn.Open();
				try
				{
					var res = cmd.ExecuteNonQuery();
				}
				catch (System.Exception ex)
				{
					LastErrorMessage = ex.Message;
				}
				finally
				{
					cn.Close();
				}
			}
		}

		public void SaveNote(DateTime forDate, string note)
		{
			string query = "delete from notes where date = @date;INSERT INTO Notes(date, note) VALUES (@date, @note);";

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@date", SqlDbType.Date).Value = forDate.Date;
				cmd.Parameters.Add("@note", SqlDbType.NVarChar).Value = note;
				cn.Open();
				try
				{
					var res = cmd.ExecuteNonQuery();
				}
				catch (System.Exception ex)
				{
					LastErrorMessage = ex.Message;
				}
				finally
				{
					cn.Close();
				}
			}
		}

		public Note GetNote(DateTime forDate)
		{
			string query = "select * from Notes where date = @date";
			Note note = null;

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@date", SqlDbType.Date).Value = forDate.Date;
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					note = Note.FromDataReader(dr);
					break;
				}
				cn.Close();
			}
			return note;
		}

		public List<DateTime>GetNoteDates(DateTime fromDate)
		{
			string query = "select date from Notes where date >= @date";
			var dates = new List<DateTime>();

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@date", SqlDbType.Date).Value = fromDate.Date;
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
					dates.Add((DateTime)dr["date"]);
				cn.Close();
			}
			return dates;
		}

		public void AddNews(string message)
		{
			string query = "INSERT INTO News(message) VALUES (@newMessage);";			

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@newMessage", SqlDbType.NVarChar).Value = message;				
				cn.Open();
				try
				{
					var res = cmd.ExecuteNonQuery();					
				}
				catch (System.Exception ex)
				{
					LastErrorMessage = ex.Message;					
				}
				finally
				{
					cn.Close();
				}
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
				cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = DateTime.Now.Date;
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
			string query = "select id,name,price,duration,abbreviation,isObsolete from Services,(SELECT serviceId FROM dbo.NailDateService where nailDateId = @ID) as t where t.serviceId = id";
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

		public List<int> GetServicesIDsForDate(int nailDateID)
		{
			string query = "select id from Services,(SELECT serviceId FROM dbo.NailDateService where nailDateId = @ID) as t where t.serviceId = id";
			var services = new List<int>();

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@ID", SqlDbType.Int).Value = nailDateID;
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
					services.Add((int)dr["id"]);
				cn.Close();
			}
			return services;
		}

		public List<NailService> GetAvailableServices()
		{
			string query = "select * from Services where isObsolete = 'False' order by pos";
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

		public List<NailDate> GetNailDatesForTimeRange(DateTime sinceTime, DateTime tillTime)
		{
			string query = "select * from NailDates where StartTime >= @sinceTime and StartTime <=@tillTime order by StartTime";
			var dates = new List<NailDate>();
			// create connection and command
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@sinceTime", SqlDbType.DateTime).Value = sinceTime;
				cmd.Parameters.Add("@tillTime", SqlDbType.DateTime).Value = tillTime.AddDays(1);
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
					dates.Add(NailDate.Parse(dr));
				cn.Close();
			}
			return dates;
		}

		private List<NailDate> GetNailDatesSinceDate(DateTime sinceTime)
		{
			string query = "select * from NailDates where StartTime >= @DateFrom";
			var dates = new List<NailDate>();
			// create connection and command
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = sinceTime;
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
					dates.Add(NailDate.Parse(dr));
				cn.Close();
			}
			return dates;
		}		

		public List<NailDate> GetNailDatesFromBeginningWeek()
		{
			return GetNailDatesSinceDate(DateTimeHelper.getStartOfCurrentWeek().Date);
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

		public bool UpdateNailDate(NailDate date,List<int> currentSelServicesIDs, List<int> oldServicesIDs)
		{
			string query = "update dbo.NailDates set StartTime = @StartTime, Duration = @Duration, ClientName = @ClientName, ClientPhone = @ClientPhone, tips = @tips where id=@ID;";
			var servWithOutDesign = currentSelServicesIDs.Where(w => w != 10);
			if (servWithOutDesign.Count() == 0)
				query += "delete from dbo.NailDateService where nailDateId = @ID;";
			else
				query += "delete from dbo.NailDateService where nailDateId = @ID and serviceId not in ("+String.Join(",", servWithOutDesign) +");";

			var tmp = oldServicesIDs.Where(w => w != 10);
			foreach (int id in tmp)
				currentSelServicesIDs.Remove(id);
			currentSelServicesIDs.ForEach(fe => query += String.Format("insert into dbo.NailDateService (nailDateId, serviceId) values (@ID, @serviceId{0});", fe));

			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cmd.Parameters.Add("@ID", SqlDbType.Int).Value = date.ID;
				cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = date.StartTime;
				cmd.Parameters.Add("@Duration", SqlDbType.BigInt).Value = date.Duration.TotalMinutes;
				cmd.Parameters.Add("@ClientName", SqlDbType.NText, 20).Value = date.ClientName;
				cmd.Parameters.Add("@ClientPhone", SqlDbType.VarChar, 15).Value = date.ClientPhone;				

				if (date.Tips.HasValue)
					cmd.Parameters.Add("@tips", SqlDbType.SmallInt).Value = date.Tips;
				else
					cmd.Parameters.AddWithValue("@tips", DBNull.Value);

				currentSelServicesIDs.Distinct().ToList().ForEach(i=> cmd.Parameters.Add(String.Format("@serviceId{0}", i), SqlDbType.Int).Value = i);
				cn.Open();
				try
				{
					cmd.ExecuteNonQuery();
				}
				catch 
				{
					return false;
				}
				finally
				{
					cn.Close();
				}
				return true;
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
				cmd.Parameters.Add("@Duration", SqlDbType.BigInt).Value = duration.TotalMinutes;
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