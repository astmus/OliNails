﻿using System;
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
		public DateTime FirstDay { get; set; }
		public event Action<DateTime> AddNewDateButtonPressed;
		List<NailDate> WeekDates { get; set; }
		public WorkWeek(DateTime firstDay, List<NailDate> weekDates)
		{
			FirstDay = firstDay;
			WeekDates = weekDates;
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
				DaysHeader.Cells.Add(new TableHeaderCell() { ForeColor = Color.White, BackColor = Color.FromArgb(1, 104, 105, 117), BorderColor = Color.Gray, BorderWidth = 1 });
				if (firstDay.Date == nowDateTime.Date)
					DaysHeader.Cells[i + 1].BackColor = Color.FromArgb(1, 159, 197, 121);
				DaysHeader.Cells[i + 1].Text = firstDay.ToString("ddd dd MMM").ToLower();
				firstDay = firstDay.AddDays(1);
			}
			this.Rows.Add(DaysHeader);
			drawWeek();
		}

		/*private DateTime firstDayOfCurrentWeek
		{
			get
			{
				DateTime nowDateTime = DateTime.UtcNow;
				DateTime newDateTime = TimeZoneInfo.ConvertTime(
					nowDateTime,
					TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
				return newDateTime.AddDays(-1 * (int)(newDateTime.DayOfWeek - 1));
			}
		}
		*/
		private DateTime nowDateTime
		{
			get
			{
				DateTime nowDateTime = DateTime.UtcNow;
				DateTime newDateTime = TimeZoneInfo.ConvertTime(
					nowDateTime,
					TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
				return newDateTime;
			}
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
					var dateCell = new TableCell() { Width = 100, Height = 30 };

					var certainTime = currentDay.Date.Add(nailTime);
					NailDate existsNailDate = WeekDates.FirstOrDefault(a => a.StartTime == certainTime);
					if (existsNailDate != null)
						backColor = Color.FromArgb(1, 228, 83, 131);
					else
					if (currentDay.Date == nowDateTime.Date)
						backColor = Color.FromArgb(1, 217, 232, 202);
					else
						backColor = odd % 2 == 0 ? Color.FromArgb(1, 233, 231, 241) : Color.FromArgb(1, 241, 239, 237);

					dateCell.BackColor = backColor;
					dateCell.BorderWidth = 1;
					dateCell.BorderColor = Color.Gray;
					dateCell.HorizontalAlign = HorizontalAlign.Center;
					dateCell.VerticalAlign = VerticalAlign.Middle;

					if (existsNailDate == null && certainTime > nowDateTime)
					{
						var b = new TagButton() { Tag = certainTime, Text = "Записаться", BackColor = backColor, Width = 100, Height = 30 };
						b.Click += onAddDateButtonClick;
						dateCell.Controls.Add(b);
					}					
					if (existsNailDate != null)
						dateCell.Controls.Add(new Literal() {Text = existsNailDate.ClientName });
					row.Cells.Add(dateCell);
					currentDay = currentDay.AddDays(1);
				}
				Rows.Add(row);
			}
		}

		private void onAddDateButtonClick(object sender, EventArgs e)
		{
			AddNewDateButtonPressed?.Invoke((DateTime)(sender as TagButton).Tag);
		}
	}
}
