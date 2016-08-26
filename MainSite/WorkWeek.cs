using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MainSite
{
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:WorkWeek runat=server></{0}:WorkWeek>")]
	public class WorkWeek : Table
	{
		public TableHeaderRow DaysHeader { get; set; }
		public DateTime FirstDay { get; set;}

		public WorkWeek(DateTime firstDay)
		{
			FirstDay = firstDay;			
			CellSpacing = 0;
			BorderWidth = 1;
			BorderColor = Color.Gray;
			DaysHeader = new TableHeaderRow();			
			DaysHeader.BorderColor = Color.Gray;

			var headerCell = new TableHeaderCell() { ForeColor = Color.White, BackColor = Color.Black };
			headerCell.Style.Add("padding", "5px");
			headerCell.Text = "время";
			DaysHeader.Cells.Add(headerCell);
			for (int i = 0; i < 7; i++)
			{
				DaysHeader.Cells.Add(new TableHeaderCell() { ForeColor = Color.White, BackColor = Color.FromArgb(1,104,105,117), BorderColor= Color.Gray, BorderWidth = 1 });
				if (firstDay.Date == DateTime.UtcNow.Date)
					DaysHeader.Cells[i+1].BackColor = Color.FromArgb(1, 159, 197, 121);
				DaysHeader.Cells[i+1].Text = firstDay.ToString("ddd dd MMM").ToLower();
				firstDay = firstDay.AddDays(1);
			}
			this.Rows.Add(DaysHeader);
			drawWeek();
		}

		public void drawWeek()
		{			
			int odd = 0;
			foreach (string time in master.timeList)
			{
				DateTime currentDay = FirstDay;
				var row = new TableRow();
				var nailTime = TimeSpan.Parse(time);
				var timeCell = new TableCell();
				timeCell.Text = time;
				timeCell.HorizontalAlign = HorizontalAlign.Center;
				timeCell.BorderWidth = 1;
				timeCell.BorderColor = Color.Gray;
				row.Cells.Add(timeCell);
				Color backColor;
				
				odd++;
				for (int i = 1; i < 8; i++)
				{
					var dateCell = new TableCell() {Width = 100, Height=30 };
					if (currentDay.Date == DateTime.UtcNow.Date)
						backColor = Color.FromArgb(1, 217, 232, 202);
					else
						backColor = odd % 2 == 0 ? Color.FromArgb(1, 233, 231, 241) : Color.FromArgb(1, 241, 239, 237);

					dateCell.BackColor = backColor;
					dateCell.BorderWidth = 1;
					dateCell.BorderColor = Color.Gray;
					var certainTime = currentDay.Add(nailTime);
					if (certainTime > DateTime.UtcNow)
					{
						var b = new TagButton();
						b.Tag = certainTime;
						b.Text = "Записаться";
						b.BackColor = backColor;
						b.Width = 100;
						b.Height = 30;
						//b.Click += onCreateNailDateClick;
						dateCell.Controls.Add(b);
					}
					row.Cells.Add(dateCell);
					currentDay = currentDay.AddDays(1);
				}
				//row.BorderWidth = 1;
				//row.BorderColor = Color.Gray;

				Rows.Add(row);
			}
		}
	}
}
