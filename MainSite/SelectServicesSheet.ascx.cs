using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;

namespace MainSite
{
	public partial class SelectServicesSheet : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
			if (Session["nailDate"] != null)
				nailDateLabel.Text = ((DateTime)Session["nailDate"]).ToString("Дата dd MMMM yyyy HH:mm");
			if (Request.Cookies["userData"] != null && IsPostBack == false && Phone.Length < 13 && String.IsNullOrEmpty(ClientName))
			{
				Phone = Request.Cookies["userData"]["phone"];
				ClientName = Server.UrlDecode(Request.Cookies["userData"]["name"]);
			}
			//this.Style.Add("transform", "scale(2,2)");
		}

		public event Action<SelectServicesSheet> UpdateNailDate;
		public event Action<SelectServicesSheet> DeleteNailDate;

		#region Properties

		public string Phone
		{
			get { return phone.Text; }
			set { phone.Text = value; }
		}

		public string ClientName
		{
			get { return clientName.Text; }
			set { clientName.Text = value; }
		}

		public DateTime StartTime
		{
			get { return (DateTime)Session["nailDate"]; }
			set
			{
				Session["nailDate"] = value;
				nailDateLabel.Text = value.ToString("Дата dd MMMM yyyy HH:mm");
			}
		}

		public bool ConfirmButtonVisibility
		{
			get { return confirmButtonsPanel.Visible; }
			set { confirmButtonsPanel.Visible = value; }
		}	

		private List<int> _selectedServicesIDs = new List<int>();
		public List<int> SelectedServicesIDs
		{
			get {
				_selectedServicesIDs.Clear();
				foreach (GridViewRow row in GridView1.Rows)
				{
					if (!(row.Cells[0].Controls[5] as CheckBox).Checked) continue;
					int serviceId = int.Parse((row.Cells[0].Controls[1] as Label).Text);
					if (serviceId == 10)
					{						
						int count = int.Parse(Request["currentCountN"]);
						_selectedServicesIDs.AddRange(Enumerable.Repeat(10, count));
					}
					else
						_selectedServicesIDs.Add(serviceId);
				}
				return _selectedServicesIDs;
			}
			set { _selectedServicesIDs = value; GridView1.DataBind(); }
		}

		#endregion

		public void SetDisplayEditDeleteButtons(bool visible)
		{
			updateDateButton.Visible = visible;
			deleteDateButton.Visible = visible;
			OkButton.Visible = !visible;
		}

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
			
			if (servicesIDs.Contains(10))
			{				
				var v = Request["currentCountN"];
				int count = int.Parse(v);
				if (count > 1)
					servicesIDs.AddRange(Enumerable.Repeat(10, count-1));
			}

			Response.Cookies["userData"]["date"] = StartTime.Ticks.ToString();
			Response.Cookies["userData"]["phone"] = Phone;
			Response.Cookies["userData"]["name"] = Server.UrlEncode(ClientName);
			Response.Cookies["userData"].Expires = StartTime;

			DataBaseHandler.Instance.InsertNailDate(StartTime, TimeSpan.Zero, ClientName, Phone, servicesIDs);

			SendMailNotification(StartTime, TimeSpan.Zero, ClientName, Phone, servicesNames);
			Task.Run(() => { Response.Redirect("/master.aspx"); });
			
			Session.Clear();			
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
			mailMsg.Body = userName + " " + Environment.NewLine + String.Join(",", selectedServicesName) + Environment.NewLine + startDate.ToString() + " тел: " + userPhon;

			SmtpClient client = new SmtpClient("smtp.mail.ru", 25);
			client.Credentials = new System.Net.NetworkCredential() { UserName = "oli_882011@mail.ru", Password = "rusaya8" };
			client.EnableSsl = true;

			client.Send(mailMsg);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			foreach (GridViewRow row in GridView1.Rows)
			{
				row.CssClass = (row.Cells[0].Controls[5] as CheckBox).Checked ? "selectedrow" : "rows";				
				if (uint.Parse((row.FindControl("procedureIdLabel") as Label).Text) == 10 && Session["designPanel"] != null)
					row.Cells[0].Controls.Add(Session["designPanel"] as Panel);
			}
		}
		
		uint _totalPrice = 0;
		protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				e.Row.Attributes["onclick"] = "selectRow(this)";
				uint id = 0;				
				if (uint.TryParse((e.Row.FindControl("procedureIdLabel") as Label).Text, out id))
				{				
					uint count = (uint)_selectedServicesIDs.Count(c => c == id);					
					if (count != 0)
					{
						(e.Row.Cells[0].Controls[5] as CheckBox).Checked = true;						
						var parts = (e.Row.Cells[1].Controls[1] as Label).Text.Split(new[] { ' ' });
						uint price = uint.Parse(parts[0]) * count;
						_totalPrice += price;
						if (id == 10)
							Session["designPrice"] = parts[0];
						(e.Row.Cells[1].Controls[1] as Label).Text = price + " " + parts[1];
						e.Row.CssClass = "selectedrow";
						Session["totalPrice"] = _totalPrice; // displaying is occur in java script							
					}
					if (id == 10)
					{
						Panel p = new Panel();
						Session["designPanel"] = p;
						p.Style.Add("display", "inline-block");
						HtmlInputButton decButt = new HtmlInputButton() { Value = "-" };
						decButt.Attributes["onclick"] = "decreaseDesign(this);event.stopPropagation(); return false;";
						HtmlInputButton incButt = new HtmlInputButton() { Value = "+", };
						incButt.Attributes["onclick"] = "increaseDesign(this);event.stopPropagation(); return false;";
						var text = new HtmlGenericControl("input readonly");
						text.Style.Add("text-align", "center");
						text.Attributes["id"] = "currentCount";
						text.Attributes["name"] = "currentCountN";
						text.Attributes["value"] = count != 0 ? count.ToString() : "1";
						text.Style.Add("width", "40px");
						text.Style.Add("margin-left", "5px");
						text.Style.Add("margin-right", "5px");
						p.Controls.Add(decButt);
						p.Controls.Add(text);
						p.Controls.Add(incButt);
						e.Row.Cells[0].Controls.Add(p);
					}
				}
			}

		}

		protected void deleteDateButton_Click(object sender, EventArgs e)
		{
			if (DeleteNailDate != null)
				DeleteNailDate(this);
		}

		protected void updateDateButton_Click(object sender, EventArgs e)
		{
			if (UpdateNailDate != null)
				UpdateNailDate(this);
		}
	}
}