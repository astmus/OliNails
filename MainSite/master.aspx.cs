using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite
{
	public partial class master : System.Web.UI.Page
	{
		private static List<string> timeList = new List<string>(){"10:00","14:00","16:00","18:00"};

		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);
			/*if (Request.Browser.IsMobileDevice)
				MasterPageFile = "~/mmaster.aspx";*/
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
			GetFutureNailDates();
			DateTime start = getStartOfCurrentWeek();
			for (int i = 0; i < 1; i++)
			{
				drawWeek(start);
				start = start.AddDays(7);
			}						
		}

		private void drawWeek(DateTime firstDay)
		{
			DateTime startOfWeek = firstDay;
			/*var daysHeader = new TableHeaderRow();
			daysHeader.BorderStyle = BorderStyle.Outset;
			daysHeader.BorderColor = Color.Gray;
			daysHeader.BorderWidth = 1;*/
			for (int i = 1; i < 8; i++)
			{
				if (startOfWeek.Date == DateTime.Now.Date)
					daysHeader.Cells[i].BackColor = Color.LightGreen;
				daysHeader.Cells[i].Text = startOfWeek.ToString("ddd dd MMM").ToLower();
				startOfWeek = startOfWeek.AddDays(1);
			}

			int odd = 0;
			foreach (string time in master.timeList)
			{
				startOfWeek = firstDay;
				var row = new TableRow();
				var nailTime = TimeSpan.Parse(time);
				var timeCell = new TableCell();
				timeCell.Text = time;
				timeCell.HorizontalAlign = HorizontalAlign.Center;
				row.Cells.Add(timeCell);
				var backColor = odd % 2 == 0 ? Color.Wheat : Color.AliceBlue;
				odd++;
				for (int i = 1; i < 8; i++)
				{
					var dateCell = new TableCell();
					dateCell.BackColor = backColor;
					dateCell.BorderWidth = 1;
					var certainTime = startOfWeek.Add(nailTime);
					if (certainTime > DateTime.Now)
					{
						var b = new TagButton();
						b.Tag = certainTime;
						b.Text = "Записаться";
						b.BackColor = backColor;
						b.Click += CreateNewNailDate;
						dateCell.Controls.Add(b);
					}
					row.Cells.Add(dateCell);
					startOfWeek = startOfWeek.AddDays(1);
				}
				row.BorderWidth = 1;
				row.BorderColor = Color.Gray;
				mainTable.Rows.Add(row);
			}
		}

		private void CreateNewNailDate(object sender, EventArgs e)
		{
			//MsgBox(((DateTime)(sender as TagButton).Tag).ToString(), this, sender);
			var date = (DateTime)(sender as TagButton).Tag;
			nailDateLabel.Text = date.ToString("Дата dd MMMM yyyy HH:mm");
			Session["nailDate"] = date;			
			mp1.Show();			
		}

		DateTime getStartOfCurrentWeek()
		{
			return DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek - 1));
		}

		public void MsgBox(String ex, Page pg, Object obj)
		{
			string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
			Type cstype = obj.GetType();
			ClientScriptManager cs = pg.ClientScript;
			cs.RegisterClientScriptBlock(cstype, s, s.ToString());
		}

		protected void AddNailDate(object sender, EventArgs e)
		{
			mp1.Hide();
			var date = Session["nailDate"] as DateTime?;
			if (date.HasValue)
			{
				Session["nailDate"] = null;
				InsertNailDate(date.Value, TimeSpan.FromHours(2), clientName.Text, phone.Text);
			}
			MsgBox("Completed", this, sender);
		}

		private void GetFutureNailDates()
		{
			string query = "select * from NailDates";

			// create connection and command
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				dr.Read();
				// open connection, execute INSERT, close connection
				cn.Close();
			}
		}

		private void InsertNailDate(DateTime startDate, TimeSpan duration, string userName, string userPhone)
		{
			// define INSERT query with parameters
			string query = "INSERT INTO dbo.NailDates (StartTime, Duration, ClientName, ClientPhone) " +
						   "VALUES (@StartTime, @Duration, @ClientName, @ClientPhone) ";

			// create connection and command
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionSctring"].ConnectionString))
			using (SqlCommand cmd = new SqlCommand(query, cn))
			{
				// define parameters and their values
				cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = startDate;
				cmd.Parameters.Add("@Duration", SqlDbType.BigInt).Value = duration.Ticks;
				cmd.Parameters.Add("@ClientName", SqlDbType.NText, 20).Value = userName;
				cmd.Parameters.Add("@ClientPhone", SqlDbType.VarChar, 15).Value = userPhone;

				// open connection, execute INSERT, close connection
				cn.Open();
				cmd.ExecuteNonQuery();
				cn.Close();
			}
		}
	}
}