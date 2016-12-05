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
using System.Web.UI.HtmlControls;
using System.Text;

namespace MainSite
{
	public partial class master : System.Web.UI.Page
	{
		public NailScheduler scheduler;
		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);
			//OkButton.Style.Add("disabled", "true");			
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
			scheduler = new NailScheduler(Settings.Instance.AvailableTimes, DataBaseHandler.Instance.GetFutureNailDates(), Mode.User);
			scheduler.CreateNailDate += OnCreateNailDate;	
			
			mainPanel.Controls.Add(scheduler);			
		}

		private void OnCreateNailDate(DateTime startTime)
		{
			Session["nailDate"] = startTime;			
			if (Request.Browser.IsMobileDevice)
				Response.Redirect("SelectSercvices.aspx");
			else
			{
				srvTable.ReloadServices();
				ShowServicesSheet();
			}
		}

		public void ShowServicesSheet()
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "CallFunc", "showModal(event)", true);			
		}

		protected void NailDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
		{
			e.Command.Parameters["@localTime"].Value = Session["nailDate"] ?? DateTimeHelper.currentLocalDateTime();
		}

		private void SendMailNotification(DateTime startDate, TimeSpan duration, string userName, string userPhon, List<string> selectedServicesName)
		{
			MailMessage mailMsg = new MailMessage();
			mailMsg.From = new MailAddress("oli_882011@mail.ru");
			mailMsg.To.Add(new MailAddress("olgas882013@gmail.com"));
			mailMsg.IsBodyHtml = false;
			mailMsg.Subject = "Запись на "+startDate.ToString();
			mailMsg.Body = userName + " желает запись на " + Environment.NewLine + String.Join(",",selectedServicesName) + Environment.NewLine + startDate.ToString() + " тел: " + userPhon;
			
			SmtpClient client = new SmtpClient("smtp.mail.ru",25);
			client.Credentials = new System.Net.NetworkCredential() { UserName = "oli_882011@mail.ru", Password = "rusaya8" };
			client.EnableSsl = true;
			
			client.Send(mailMsg);
		}

		public void ShowAlertBox(string message)
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('" + message + "');", true);
		}

		private void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			
		}

		protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
		{			
			if (e.Row.RowType == DataControlRowType.DataRow)
				e.Row.Attributes["onclick"] = "selectRow(this)";
		}
	}
}