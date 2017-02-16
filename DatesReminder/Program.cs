using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DatesReminder
{
    class Program
    {
        static void Main(string[] args)
        {
            var infos = GetDummyUserRemindInfo();
            foreach (var info in infos)
                SendSms(info);
        }

        private static void SendSms(UserRemindInfo info)
        {

        }

        public static List<UserRemindInfo> GetDummyUserRemindInfo()
        {
            List<UserRemindInfo> result = new List<UserRemindInfo>();
            result.Add(new UserRemindInfo() { Name = "Мах", PhoneNumber = "+380668851043", Time = "10:00" });
            result.Add(new UserRemindInfo() { Name = "Мах", PhoneNumber = "+380939372715", Time = "13:00" });
            result.Add(new UserRemindInfo() { Name = "Мах", PhoneNumber = "+380668851043", Time = "17:00" });
            return result;
        }

        public static List<UserRemindInfo> GetUserRemindInfo()
        {
            List<UserRemindInfo> result = new List<UserRemindInfo>(); 
            string query = "select * from InfoAboutClientsForTomorrow";
            // create connection and command
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        UserRemindInfo info = new UserRemindInfo();
                        info.Name = (string)dr["ClientName"];
                        info.PhoneNumber = (string)dr["ClientPhone"];
                        info.Time = ((DateTime)dr["StartTime"]).ToString("HH:mm");
                        result.Add(info);
                    }
                    //   dates = NailDate.Parse(dr);
                }
                catch (System.Exception ex)
                {
                }
                finally
                {
                    cn.Close();                    
                }
                return result;
            }
        }
    }

    struct UserRemindInfo
    {
        public string Name;
        public string PhoneNumber;
        public string Time;
    }
}
