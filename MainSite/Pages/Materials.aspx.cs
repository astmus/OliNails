using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite.Pages
{
	public partial class Materials : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				if (Cache["materialsCacheKey"] == null)
					Cache["materialsCacheKey"] = DateTime.Now;
		}

		protected void table_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType != DataControlRowType.DataRow) return;
			
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

		string editMaterialId = null;
		protected void materialTable_RowEditing(object sender, GridViewEditEventArgs e)
		{
			var gridView = sender as GridView;
			gridView.Rows[e.NewEditIndex].Attributes.Remove("onclick");
			editMaterialId = ((DataView)materialDataSource.Select(DataSourceSelectArguments.Empty)).Table.Rows[e.NewEditIndex][0].ToString();
			gridView.SelectedIndex = e.NewEditIndex;
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
				case "Insert":
					materialDataSource.Insert();
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
			DataBaseHandler.Instance.UpdateRelatedToMaterialServices(int.Parse(materialId), selectedServicesIDs);
			selectServicesTable.Visible = false;
			Cache["materialsCacheKey"] = DateTime.Now;
			useMaterialTable.DataBind();
		}

		protected void materialTable_RowCreated(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != DataControlRowState.Edit)
				e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.materialTable, "Select$" + e.Row.RowIndex);
		}

		protected void onInsertNewMaterialClick(object sender, EventArgs e)
		{
			materialDataSource.Insert();
			//Cache["materialsCacheKey"] = DateTime.Now;
			materialTable.DataBind();
		}

		protected void materialDataSource_Inserting(object sender, SqlDataSourceCommandEventArgs e)
		{
			e.Command.Parameters["@name"].Value = (materialTable.FooterRow.FindControl("nameBox") as TextBox).Text;
			e.Command.Parameters["@price"].Value = Int16.Parse((materialTable.FooterRow.FindControl("priceTextBox") as TextBox).Text);
			e.Command.Parameters["@amount"].Value = Int16.Parse((materialTable.FooterRow.FindControl("amountTextBox") as TextBox).Text);
			e.Command.Parameters["@startTime"].Value = DateTime.Parse((materialTable.FooterRow.FindControl("sinceTimeBox") as TextBox).Text);
		}
	}
}