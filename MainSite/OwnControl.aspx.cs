using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite
{
	public partial class OwnControl : System.Web.UI.Page
	{
		NailScheduler scheduler;
		protected void Page_Load(object sender, EventArgs e)
		{
			Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
			//GetFutureNailDates();	
			scheduler = new NailScheduler(Settings.Instance.AvailableTimes, DataBaseHandler.Instance.GetFutureNailDates(), Mode.Owner);
			scheduler.CreateNailDate += OnCreateNailDate;
			mainPanel.Controls.Add(scheduler);			
		}

		private void OnCreateNailDate(DateTime startTime)
		{
			throw new NotImplementedException();
		}
	}
}