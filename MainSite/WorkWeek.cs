using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MainSite.Styles;

namespace MainSite
{
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:WorkWeek runat=server></{0}:WorkWeek>")]
	public class WorkWeek : Table
	{
		public TableHeaderRow DaysHeader { get; set; }
		public DateTime FirstDay { get; set; }
		public Action<DateTime> AddNewDateButtonPressed;
		public Action<NailDate> NailDateSelected;
		public Action<DateTime> ReservDatePressed;
		List<NailDate> WeekDates { get; set; }
		private Mode _currentMode { get; set; }
		public WorkWeek(DateTime firstDay, List<NailDate> weekDates, Mode workForMode,List<DateTime> noteDates)
		{
			FirstDay = firstDay;
			WeekDates = weekDates;
			CellSpacing = 0;
			CellPadding = 0;			
			BorderWidth = 1;
			BorderColor = Color.Black;
			BorderStyle = BorderStyle.Solid;
			DaysHeader = new TableHeaderRow();
			DaysHeader.BorderColor = Color.Gray;
			_currentMode = workForMode;
			var headerCell = new TableHeaderCell();// { ForeColor = Color.White, BackColor = Color.Black };
			headerCell.Style.Add("padding", "5px");
			headerCell.Text = "время";
			DaysHeader.Cells.Add(headerCell);
			this.CssClass = "workWeek";
			for (int i = 0; i < 7; i++)
			{
				var cell = new TableHeaderCell();// { ForeColor = Color.White, BackColor = StyleColors.TableHeaderBackground, BorderColor = Color.Gray, BorderWidth = 1 };
				DaysHeader.Cells.Add(cell);
				if (firstDay.Date == nowDateTime.Date)
					cell.BackColor = StyleColors.Active;
				cell.Text = firstDay.ToString("ddd dd MMM").ToLower();
				if (_currentMode == Mode.Owner)
					if (noteDates.Contains(firstDay.Date))
						addNoteMark(cell, firstDay);
					else
						cell.Attributes.Add("onclick", "HandleIT(" + (int)TimeSpan.FromTicks(firstDay.Ticks).TotalDays + "); return false");

				firstDay = firstDay.AddDays(1);
			}
			this.Rows.Add(DaysHeader);
			drawWeek();
		}

		private void addNoteMark(TableHeaderCell cell, DateTime date)
		{
			Panel cellContent = new Panel();
			int countDays = (int)TimeSpan.FromTicks(date.Ticks).TotalDays;
			cellContent.Attributes.Add("onclick", "HandleIT("+countDays+"); return false");

			HtmlGenericControl div = new HtmlGenericControl();
			div.TagName = "div";
			div.Attributes.Add("class", "hasNote");			
			cellContent.Controls.Add(div);			
			var lit = new Label { Text = date.ToString("ddd dd MMM").ToLower() };
			lit.Style.Add("posiion", "absolute");
			cellContent.Controls.Add(lit);

			cell.Controls.Add(cellContent);
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
			foreach (string time in Settings.Instance.AvailableTimes)
			{
				DateTime currentDay = FirstDay;
				var row = new TableRow();
				var nailTime = TimeSpan.Parse(time);
				var timeCell = new TableCell();
				timeCell.Text = time;
				timeCell.CssClass = "timeCell";
				row.Cells.Add(timeCell);

				odd++;
				for (int i = 1; i < 8; i++)
				{
					var dateCell = new TableCell();// { Width = 100, Height = 30 };

					var certainTime = currentDay.Date.Add(nailTime);
					NailDate existsNailDate = WeekDates.FirstOrDefault(a => a.StartTime == certainTime);
					if (existsNailDate != null && (existsNailDate.StartTime > nowDateTime || _currentMode == Mode.Owner))
						dateCell.CssClass = "reserved";//backColor = StyleColors.Reserved;
					else
					if (currentDay.Date == nowDateTime.Date)
						dateCell.CssClass = "active";//backColor = StyleColors.LightActive;
					else
						dateCell.CssClass = odd % 2 == 0 ? "evenCell" : "notEvenCell";//backColor = odd % 2 == 0 ? StyleColors.EvenLines : StyleColors.NotEvenLines;

					Control innerControl = null;

					switch (_currentMode)
					{
						case Mode.User:
							innerControl = GenerateContentForUserCell(existsNailDate, certainTime);
							break;							
						case Mode.Owner:
							innerControl = GenerateContentForOwnerCell(existsNailDate, certainTime);
							break;
					}

					if (innerControl != null)
						dateCell.Controls.Add(innerControl);
					row.Cells.Add(dateCell);
					currentDay = currentDay.AddDays(1);
				}
				Rows.Add(row);
			}
		}

		private Control GenerateContentForOwnerCell(NailDate existsNailDate, DateTime certainTime)
		{
			var b = new TagButton() { Tag = existsNailDate };
			b.Click += onButtonPressedByOwner; ;
			if (existsNailDate != null)
			{
				b.Text = existsNailDate.ClientName;
				//b.BackColor = Color.FromArgb(1, 228, 83, 131);
			}
			else
			{
				b.Tag = certainTime;
				b.Text = certainTime > nowDateTime ? "Блокировать" : "Добавить";
			}
			return b;
		}

		private void onButtonPressedByOwner(object sender, EventArgs e)
		{
			object data = (sender as TagButton).Tag;
			if (data is DateTime)
				ReservDatePressed?.Invoke((DateTime)data);
			if (data is NailDate)
				NailDateSelected?.Invoke(data as NailDate);
		}

		private Control GenerateContentForUserCell(NailDate existsNailDate, DateTime certainTime)
		{
			Control result = null;
			if (certainTime <= nowDateTime)
				return null;
			if (existsNailDate == null)
			{
				var b = new TagButton() { Tag = certainTime, Text = "Записаться" };
				//b.Attributes.Add("time", certainTime.ToString("Дата dd MMMM yyyy HH:mm"));
				//b.UseSubmitBehavior = false;
				//b.OnClientClick = "showModal(event); return false;";
				b.Click += onAddDateButtonClick;				
				result = b;
			}
			else
				result = new Literal() { Text = "Занято" };
			return result;
		}

		private void onAddDateButtonClick(object sender, EventArgs e)
		{
			AddNewDateButtonPressed?.Invoke((DateTime)(sender as TagButton).Tag);
		}
	}
}
