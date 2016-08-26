using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite
{
	public partial class master : System.Web.UI.Page
	{
		public static List<string> timeList = new List<string>(){"10:00","14:00","16:00","18:00"};
		public NailScheduler scheduler;
		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);
			/*if (Request.Browser.IsMobileDevice)
				MasterPageFile = "~/mmaster.aspx";*/
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
			//GetFutureNailDates();	
			scheduler = new NailScheduler(timeList, GetFutureNailDates());
			scheduler.CreateNailDate += OnCreateNailDate;
			mainPanel.Controls.Add(scheduler);
			if (Request.Browser.IsMobileDevice)
				Panl1.Style.Add("transform", "scale(3,3)");
		}

		private void OnCreateNailDate(DateTime startTime)
		{
			nailDateLabel.Text = startTime.ToString("Дата dd MMMM yyyy HH:mm");
			Session["nailDate"] = startTime;
			mp1.Show();
		}

		public void MsgBox(String ex, Page pg, Object obj)
		{
			string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
			Type cstype = obj.GetType();
			ClientScriptManager cs = pg.ClientScript;
			cs.RegisterClientScriptBlock(cstype, s, s.ToString());
		}

		protected void AddNailDate(object sender, EventArgs e)
		{
			mp1.Hide();
			var date = Session["nailDate"] as DateTime?;
			if (date.HasValue)
			{
				Session["nailDate"] = null;
				InsertNailDate(date.Value, TimeSpan.FromHours(2), clientName.Text, phone.Text);
			}
			Response.Redirect(Request.RawUrl);
		}

		private List<NailDate> GetFutureNailDates()
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

		private void InsertNailDate(DateTime startDate, TimeSpan duration, string userName, string userPhone)
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