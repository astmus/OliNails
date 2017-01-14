using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite.Pages
{
	public partial class Materials : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void table_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType != DataControlRowType.DataRow) return;
			if (sender == materialTable)
			{
				if (e.Row.RowState != DataControlRowState.Edit)
					e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(materialTable, "Select$" + e.Row.RowIndex);
			}
			else
			{
				e.Row.Attributes["onclick"] = "selectRow(this,event)";
				useMaterialDataSource.SelectParameters["materialId"].DefaultValue = editMaterialId;
				var values = ((DataView)useMaterialDataSource.Select(DataSourceSelectArguments.Empty)).Table.Rows.Cast<DataRow>().Select(s => s[1].ToString());
				if (values.Contains(e.Row.Cells[0].Text))
				{
					e.Row.CssClass = "selectedrow";
					(e.Row.Cells[1].Controls[1] as CheckBox).Checked = true;
				}
				else
				{
					e.Row.CssClass = "rows";
					(e.Row.Cells[1].Controls[1] as CheckBox).Checked = false;
				}
			}
		}

		protected void table_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		protected void useMaterialDataSource_Selected(object sender, SqlDataSourceStatusEventArgs e)
		{
			
		}

		string editMaterialId = null;
		protected void materialTable_RowEditing(object sender, GridViewEditEventArgs e)
		{
			materialTable.Rows[e.NewEditIndex].Attributes["onclick"] = null;
			editMaterialId = ((DataView)materialDataSource.Select(DataSourceSelectArguments.Empty)).Table.Rows[e.NewEditIndex][0].ToString();
			materialTable.SelectedIndex = e.NewEditIndex;
			selectServicesTable.Visible = true;
			selectServicesTable.DataBind();		
		}

		protected void materialTable_RowCommand(object sender, GridViewCommandEventArgs e)
		{

			switch (e.CommandName.ToString())
			{
				case "New":
					{
						int rowIndex = int.Parse((string)e.CommandArgument);
						string val = materialTable.DataKeys[rowIndex]["id"].ToString();
						DataBaseHandler.Instance.StartNewMaterial(val);
						materialTable.DataBind();
					}
					break;
				case "Cancel":
					selectServicesTable.Visible = false;
					break;
			}
		}

		protected void materialTable_RowUpdating(object sender, GridViewUpdateEventArgs e)
		{
			var materialId = materialTable.DataKeys[e.RowIndex]["id"].ToString();
			var srow = materialTable.Rows[e.RowIndex];			
			var selectedServicesIDs = new List<string>();
			foreach (GridViewRow row in selectServicesTable.Rows)
			{
				if (!(row.FindControl("selectedServiceBox") as CheckBox).Checked) continue;				
				selectedServicesIDs.Add(row.Cells[0].Text);
			}
			DataBaseHandler.Instance.Update
		}
	}
}