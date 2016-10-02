using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MainSite
{
	public class TagButton : Button
	{
		public object Tag { get; set; }
	}
}