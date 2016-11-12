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
			
			//AddServicesToDialogTable();
			if (Request.Browser.IsMobileDevice)
			{
				dialogTable.Style.Add("transform", "scale(2,2)");
				Panl1.Style["padding-top"] = "200px";
				//scheduler.Style.Add(HtmlTextWriterStyle.Width,"100%");
			}
		}

		private void OnCreateNailDate(DateTime startTime)
		{
			nailDateLabel.Text = startTime.ToString("Дата dd MMMM yyyy HH:mm");
			Session["nailDate"] = startTime;
			GridView1.DataBind();
			ShowServicesSheet();
		}

		public void ShowServicesSheet()
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "CallFunc", "showModal(event)", true);			
		}

		protected void AddNailDate(object sender, EventArgs e)
		{
			// Select the checkboxes from the GridView control
			var servicesIDs = new List<int>();
			var servicesNames = new List<string>();
			foreach (GridViewRow row in GridView1.Rows)
			{		
				bool isChecked = ((CheckBox)row.FindControl("procedureRowSelect")).Checked;
				if (isChecked)
				{
					servicesIDs.Add(int.Parse(((Label)row.FindControl("procedureIdLabel")).Text));
					servicesNames.Add(((Label)row.FindControl("procedureAbbreviation")).Text);					
				}
			}		
			//TODO: apply validation from http://stackoverflow.com/questions/1228112/how-do-i-make-a-checkbox-required-on-an-asp-net-form						
			DateTime result = (DateTime)Session["nailDate"];						
			DataBaseHandler.Instance.InsertNailDate(result, TimeSpan.FromHours(2), clientName.Text, phone.Text, servicesIDs);

			Task.Run(() => { Response.Redirect(Request.RawUrl); });
						
			Session.Clear();
			SendMailNotification(result, TimeSpan.FromHours(2), clientName.Text, phone.Text, servicesNames);
		}

		private void AddServicesToDialogTable()
		{
			//select selected services for date select * from Services where Services.id in (SELECT serviceId FROM dbo.NailDateService where nailDateId = 107)

			var services = DataBaseHandler.Instance.GetAvailableServices();
			HtmlTable table = dialogTable as HtmlTable;
			var checks = new List<HtmlInputCheckBox>();
			foreach (var service in services)
			{
				HtmlTableRow row = new HtmlTableRow();
				HtmlInputCheckBox box = new HtmlInputCheckBox();				
				var cell = new HtmlTableCell();
				cell.ColSpan = 2;
				cell.Controls.Add(box);
				box.ID = service.ID.ToString();
				checks.Add(box);
				var title = new Literal() { Text = service.Name };
				cell.Controls.Add(title);
				row.Cells.Add(cell);
				table.Rows.Insert(table.Rows.Count - 1, row);
			}
			Session["checks"] = checks;
			Session["services"] = services;
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