using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite
{
	public partial class EditServices : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			int i = 0;
			Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
		}

		protected void GridView1_PageIndexChanged(object sender, EventArgs e)
		{
			int i = 0;
		}

		

		protected void OnAddNewServiceClick(object sender, EventArgs e)
		{

		}

		protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			
			int i = 0;
		}

		protected void NailDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
		{
			e.Command.Parameters["@localTime"].Value = DateTimeHelper.currentLocalDateTime();
		}

		protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
		{			
			int i = 0;
		}

		protected void NailDataSource_Updating(object sender, SqlDataSourceCommandEventArgs e)
		{
			bool shouldShowWarning = e.Command.Parameters["@newPrice"].Value == null;
			e.Command.Parameters["@price"].Value = e.Command.Parameters["@newPrice"].Value ?? 0;
			e.Command.Parameters.Remove(e.Command.Parameters["@newPrice"]);
			if (shouldShowWarning)
				ShowAlertBox("Внимание новая цена установлена равной нулю!!!");
			
		}

		protected void NailDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
		{
			string s = e.Command.Parameters["@result"].Value.ToString();
			ShowAlertBox(s);
		}

		public void ShowAlertBox(string message)
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('" + message + "');", true);
		}
	}
}