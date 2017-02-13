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
                        
        }

        public static void GetNailDateById(int nailDateId)
        {
            string query = "select * from InfoAboutClientsForTomorrow";
            // create connection and command
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                try
                {
                    dr.Read();
                 //   dates = NailDate.Parse(dr);
                }
                catch (System.Exception ex)
                {
                }
                finally
                {
                    cn.Close();
                }
            }
        }
    }

    struct UserRemindInfo
    {
        string Name;
        string PhoneNumber;
        string Time;
    }
}
