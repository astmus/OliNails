using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;

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

			Task.Run(() => { Response.Redirect(Request.RawUrl); });

			SendMailNotification(date.Value, TimeSpan.FromHours(2), clientName.Text, phone.Text);
		}

		private void SendMailNotification(DateTime startDate, TimeSpan duration, string userName, string userPhon)
		{
			MailMessage mailMsg = new MailMessage();
			mailMsg.From = new MailAddress("oli_882011@mail.ru");
			mailMsg.To.Add(new MailAddress("olgas882013@gmail.com"));
			mailMsg.IsBodyHtml = false;
			mailMsg.Subject = "Запись на "+startDate.ToString();
			mailMsg.Body = userName + " желает запись на " + startDate.ToString() + " тел: " + userPhon;
			
			SmtpClient client = new SmtpClient("smtp.mail.ru",25);
			client.Credentials = new System.Net.NetworkCredential() { UserName = "oli_882011@mail.ru", Password = "rusaya8" };
			client.EnableSsl = true;
			
			client.Send(mailMsg);
		}

		private void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			
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