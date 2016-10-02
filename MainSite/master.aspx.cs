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
			{
				dialogTable.Style.Add("transform", "scale(2,2)");
				Panl1.Style["padding-top"] = "200px";
			}
		}

		private void OnCreateNailDate(DateTime startTime)
		{
			nailDateLabel.Text = startTime.ToString("Дата dd MMMM yyyy HH:mm");
			Session["nailDate"] = startTime;
			MsgBox();
		}

		public void MsgBox()
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showModal()", true);			
		}

		protected void AddNailDate(object sender, EventArgs e)
		{			
			var dateStr = hiddenField.Value;
			dateStr = dateStr.Replace("Дата ", "");
			DateTime result;
			if (DateTime.TryParse(dateStr, out result) == false) return;
		
			DataBaseHandler.Instance.InsertNailDate(result, TimeSpan.FromHours(2), clientName.Text, phone.Text);

			Task.Run(() => { Response.Redirect(Request.RawUrl); });

			SendMailNotification(result, TimeSpan.FromHours(2), clientName.Text, phone.Text);
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