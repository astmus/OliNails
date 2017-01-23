using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite
{
	public partial class SelectSercvices : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			//sheet.DataBind();
			if (Session["nailDateForEdit"] != null && IsPostBack == false)
			{
				services.SetDisplayEditDeleteButtons(true);
				NailDate date = Session["nailDateForEdit"] as NailDate;
				services.StartTime = date.StartTime;
				services.ClientName = date.ClientName;
				services.Phone = date.ClientPhone;
				var ids = DataBaseHandler.Instance.GetServicesIDsForDate(date.ID);
				services.SelectedServicesIDs = ids;
				Session["oldSelectedServices"] = ids;
			}
			services.UpdateNailDate += OnUpdateNailDateClick;
			services.DeleteNailDate += OnDeleteNailDateClick;
		}

		private void OnDeleteNailDateClick(SelectServicesSheet obj)
		{
			NailDate date = Session["nailDateForEdit"] as NailDate;
			DataBaseHandler.Instance.DropNailDate(date);
			ShowAlertBox("Вы отменили запись, пожалуйста возвращайтесь к нам:)");			
		}

		private void OnUpdateNailDateClick(SelectServicesSheet obj)
		{
			NailDate date = Session["nailDateForEdit"] as NailDate;
			date.ClientName = services.ClientName;
			date.ClientPhone = services.Phone;			
			var oldServices = (Session["oldSelectedServices"] as List<int>);
			bool result = DataBaseHandler.Instance.UpdateNailDate(date, services.SelectedServicesIDs, oldServices);
			Session.Clear();
			if (result)
				ShowAlertBox("Ваша запись успешно обновлена");
			else
				ShowAlertBox("Не удалось обновить запись. Пожалйста свяжитесь с нами по тел: +380939372858 или +380953464708");				
		}

		public void ShowAlertBox(string message)
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", @"alert('" + message + "');window.location = 'master.aspx';", true);
		}
	}
}