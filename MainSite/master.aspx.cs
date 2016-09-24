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
			scheduler = new NailScheduler(Settings.Instance.AvailableTimes, DataBaseHandler.Instance.GetFutureNailDates(), Mode.User);
			scheduler.CreateNailDate += OnCreateNailDate;
			mainPanel.Controls.Add(scheduler);
			if (Request.Browser.IsMobileDevice)
				Panl1.Style.Add("transform", "scale(2,2)");
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
				DataBaseHandler.Instance.InsertNailDate(date.Value, TimeSpan.FromHours(2), clientName.Text, phone.Text);
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
	}
}