using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace MainSite
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{

		}

		protected void Session_Start(object sender, EventArgs e)
		{
			string address = HttpContext.Current.Request.UserHostAddress.ToString();
			Application.Lock();
			Logger.Instance.LogDebug(DateTimeHelper.currentLocalDateTime().ToString() + address);
			if (Application.AllKeys.Contains("countOfVisitors") == false)
				Application.Add("countOfVisitors", 0);
			if (!address.Contains(@"109.254.70.107"))
				Application["countOfVisitors"] = (int)Application["countOfVisitors"] + 1;

			Application.UnLock();
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}