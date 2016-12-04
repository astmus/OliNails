using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite
{
	public partial class SelectServicesSheet : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
			if (Session["nailDate"] != null)
				nailDateLabel.Text = ((DateTime)Session["nailDate"]).ToString("Дата dd MMMM yyyy HH:mm");			
			//this.Style.Add("transform", "scale(2,2)");
		}

#region Properties
		public string Phone
		{
			get { return phone.Text; }
		}
#endregion

		public void ShowServicesSheet()
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "CallFunc", "showModal(event)", true);
		}

		public void ReloadServices()
		{
			if (Session["nailDate"] != null)
				nailDateLabel.Text = ((DateTime)Session["nailDate"]).ToString("Дата dd MMMM yyyy HH:mm");
			GridView1.DataBind();
		}

		protected void GoBack(object sender, EventArgs e)
		{
			Response.Redirect(Request.UrlReferrer.ToString());
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
			DateTime result = (DateTime)Session["nailDate"];
			DataBaseHandler.Instance.InsertNailDate(result, TimeSpan.FromHours(2), clientName.Text, phone.Text, servicesIDs);

			Task.Run(() => { Response.Redirect("/master.aspx"); });

			Session.Clear();
			SendMailNotification(result, TimeSpan.FromHours(2), clientName.Text, phone.Text, servicesNames);
		}

		//private void AddServicesToDialogTable()
		//{
		//	//select selected services for date select * from Services where Services.id in (SELECT serviceId FROM dbo.NailDateService where nailDateId = 107)

		//	var services = DataBaseHandler.Instance.GetAvailableServices();
		//	HtmlTable table = dialogTable as HtmlTable;
		//	var checks = new List<HtmlInputCheckBox>();
		//	foreach (var service in services)
		//	{
		//		HtmlTableRow row = new HtmlTableRow();
		//		HtmlInputCheckBox box = new HtmlInputCheckBox();
		//		var cell = new HtmlTableCell();
		//		cell.ColSpan = 2;
		//		cell.Controls.Add(box);
		//		box.ID = service.ID.ToString();
		//		checks.Add(box);
		//		var title = new Literal() { Text = service.Name };
		//		cell.Controls.Add(title);
		//		row.Cells.Add(cell);
		//		table.Rows.Insert(table.Rows.Count - 1, row);
		//	}
		//	Session["checks"] = checks;
		//	Session["services"] = services;
		//}

		protected void NailDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
		{
			if (Session["nailDate"] != null)
				e.Command.Parameters["@localTime"].Value = Session["nailDate"];
			else
				e.Cancel = true;
		}

		private void SendMailNotification(DateTime startDate, TimeSpan duration, string userName, string userPhon, List<string> selectedServicesName)
		{
			MailMessage mailMsg = new MailMessage();
			mailMsg.From = new MailAddress("oli_882011@mail.ru");
			mailMsg.To.Add(new MailAddress("olgas882013@gmail.com"));
			mailMsg.IsBodyHtml = false;
			mailMsg.Subject = "Запись на " + startDate.ToString();
			mailMsg.Body = userName + " желает запись на " + Environment.NewLine + String.Join(",", selectedServicesName) + Environment.NewLine + startDate.ToString() + " тел: " + userPhon;

			SmtpClient client = new SmtpClient("smtp.mail.ru", 25);
			client.Credentials = new System.Net.NetworkCredential() { UserName = "oli_882011@mail.ru", Password = "rusaya8" };
			client.EnableSsl = true;

			client.Send(mailMsg);
		}

		//public void ShowAlertBox(string message)
		//{
		//	Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('" + message + "');", true);
		//}

		//private void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		//{

		//}

		protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
				e.Row.Attributes["onclick"] = "selectRow(this)";
		}
	}
}