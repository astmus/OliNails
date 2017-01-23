using System;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite
{
	public partial class master : System.Web.UI.Page
	{
		public NailScheduler scheduler;
		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);			
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			//Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;

			string val = null;
			if (Request.Cookies["userData"] != null)
				val = Server.HtmlEncode(Request.Cookies["userData"]["date"]);
		    DateTime date = DateTime.MinValue;
			if (!String.IsNullOrEmpty(val))
				date = new DateTime(long.Parse(val));			

			scheduler = new NailScheduler(Settings.Instance.AvailableTimes, DateTimeHelper.getStartOfCurrentWeek(), Mode.User, date);
			scheduler.CreateNailDate += OnCreateNailDate;	
			
			mainPanel.Controls.Add(scheduler);
			Logger.Instance.LogInfo("page loaded");					
		}

		private void OnCreateNailDate(DateTime startTime)
		{
			Logger.Instance.LogInfo(String.Format("OnCreateNailDate({0})",startTime.ToString("dd.MM.yyyy hh:mm")));
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
			Logger.Instance.LogInfo("ShowServicesSheet()");
			Page.ClientScript.RegisterStartupScript(this.GetType(), "CallFunc", "showModal(event)", true);			
		}

		protected void NailDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
		{
			e.Command.Parameters["@localTime"].Value = Session["nailDate"] ?? DateTimeHelper.currentLocalDateTime();
			Logger.Instance.LogInfo("NailDataSource_Selecting localTime = "+e.Command.Parameters["@localTime"].Value.ToString());
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

		[WebMethod(EnableSession = true)]
		public static void CheckEditNailDate(int dateId, string enteredPhone)
		{			
			var nailDate = DataBaseHandler.Instance.GetNailDateById(dateId);
			if (nailDate == null)
				Logger.Instance.LogError(String.Format("CheckEditNailDate dateId({0}) not found", dateId));
			else
			if (enteredPhone != nailDate.ClientPhone)
				throw new ValidatePhoneForEditException(dateId, enteredPhone);
			else
			{
				HttpContext.Current.Session["nailDateForEdit"] = nailDate;
			}
		}
	}

	public class ValidatePhoneForEditException : Exception
	{
		public ValidatePhoneForEditException(int nailDateId, string enteredPhone)
			: base(String.Format("Failed walidate phone for edit nail date. NailDateId => {0}; Entered phone => {1}", nailDateId, enteredPhone))
		{ 
		}
	}
}