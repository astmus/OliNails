using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite
{
	public partial class mmaster : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			scheduler.AwailableTimes = Settings.Instance.AvailableTimes.Select(s => TimeSpan.Parse(s)).ToList();				
		}        

		protected void scheduler_SelectionChanged(List<NailDate> obj)
		{
			buttonsPanel.Visible = true;
			foreach (string time in Settings.Instance.AvailableTimes)
			{
				TimeSpan span = TimeSpan.Parse(time);
				NailDate date = null;
				Button b = new Button();
				if ((date = obj.FirstOrDefault(a => a.StartTime.TimeOfDay == span)) != null)
				{
					b.Text = "Занято/Изменить";
					b.CssClass = "reserved";
					b.OnClientClick = "showEnterPhonePrompt(this.getAttribute('dateid'))";
					b.UseSubmitBehavior = false;
					b.Attributes.Add("dateid", date.ID.ToString());
				}
				else
				{
					b.Text = "Записаться на " + time;
					b.CssClass = "notreserved";
                    b.OnClientClick = string.Format("addNewNailDate('{0}')", (scheduler.SelectedDate.Add(TimeSpan.Parse(time)) - DateTime.MinValue).TotalMinutes);
					b.UseSubmitBehavior = false;
				}
                buttonsPanel.Controls.Add(b);
            }
        }

        [WebMethod(EnableSession = true)]
        public static void AddNewNailDate(int startTimeInMinutes)
        {
            HttpContext.Current.Session["nailDate"] = new DateTime().AddMinutes(startTimeInMinutes);
            //navigation to select services page occur in javascript            
        }

        [WebMethod(EnableSession = true)]
		public static void CheckEditNailDate(int dateId, string enteredPhone)
		{
			master.CheckEditNailDate(dateId, enteredPhone);
		}
	}
}