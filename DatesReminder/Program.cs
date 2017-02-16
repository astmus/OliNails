using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using NLog;
using NLog.Targets;
using NLog.Config;
using System.Diagnostics;
using System.Threading;

namespace DatesReminder
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureLog();
            var log = LogManager.GetCurrentClassLogger();            
            log.Debug("---------------------Application started---------------------");
            log.Trace("Request planed dates for "+DateTime.Now.Date.AddDays(1).ToString("dd.MM.yyyy"));
            var infos = GetUserRemindInfo();
            log.Trace("Recieved {0} items = {1}",infos.Count, String.Join("|",infos.Select(s=>s.ToString())));
            foreach (var info in infos)
                SendSms(info);
            log.Trace("send messages completed");
            log.Debug("---------------------Application closed---------------------");
        }        

        private static void ConfigureLog()
        {
            var config = new LoggingConfiguration();          

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            // Step 3. Set target properties 
            fileTarget.Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}";
            fileTarget.FileName = "${basedir}/log.txt";

            // Step 4. Define rules            
            var rule2 = new LoggingRule("*", LogLevel.Trace, LogLevel.Fatal, fileTarget);
            config.LoggingRules.Add(rule2);
            
            // Step 5. Activate the configuration
            LogManager.Configuration = config;            
        }

        private static void SendSms(UserRemindInfo info)
        {
            var log = LogManager.GetCurrentClassLogger();
            log.Trace("send sms start for info: " + info.ToString());
            try
            {
                Console.WriteLine("send sms to " + info.Name);
                ProcessStartInfo inf = new ProcessStartInfo(@"C:\platform-tools\sendremind.bat", string.Format("{0} \"{1}\" \"{2}\"", info.PhoneNumber, "Напоминаем завтра у вас запись на", info.Time));
                inf.CreateNoWindow = true;
                inf.WindowStyle = ProcessWindowStyle.Hidden;
                var proc = System.Diagnostics.Process.Start(inf);
                proc.WaitForExit();
                proc.Close();
                Thread.Sleep(500);
            }
            catch (System.Exception ex)
            {
                log.Error("call bat file throw exception: "+ex.Message);	
            }            
            log.Trace("send sms exit");
        }

        public static List<UserRemindInfo> GetDummyUserRemindInfo()
        {
            List<UserRemindInfo> result = new List<UserRemindInfo>();
            result.Add(new UserRemindInfo() { Name = "Мах", PhoneNumber = "+380939372715", Time = "13:00 Максим" });            
            return result;
        }

        public static List<UserRemindInfo> GetUserRemindInfo()
        {
            try
            {
                LogManager.GetCurrentClassLogger().Trace("start request planned dates from" + ConfigurationManager.AppSettings["dbConnectionSctring"]);
            }
            catch (System.Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error("cant read connection string message: "+ex.Message);
            }
            
            List<UserRemindInfo> result = new List<UserRemindInfo>(); 
            string query = "select * from InfoAboutClientsForTomorrow";
            // create connection and command
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.AppSettings["dbConnectionSctring"]))
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
                    var log = LogManager.GetCurrentClassLogger();
                    log.Error("Can not get data from DB reason:" + ex.Message);
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

        public override string ToString()
        {
            return String.Format("{0};{1};{2}",Name,PhoneNumber,Time);
        }
    }
}
