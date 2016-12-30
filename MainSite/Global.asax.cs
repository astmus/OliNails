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
			Logger.Instance.LogInfo("session start");			
			if (address != "109.254.70.107")
				Logger.Instance.LogUserCount();
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
			Exception exc = Server.GetLastError();
			Logger.Instance.LogError("message = "+exc.Message);
			Logger.Instance.LogError("inner message = " + exc.InnerException.Message);
		}

		protected void Session_End(object sender, EventArgs e)
		{
			Logger.Instance.LogInfo("session end");
		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}