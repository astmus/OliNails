using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite.Pages
{
	public partial class News : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			
		}

		protected void Add_Click(object sender, EventArgs e)
		{
			string message = newMessage.Text;
			DataBaseHandler.Instance.AddNews(message);
			GridView1.DataBind();
		}
	}
}