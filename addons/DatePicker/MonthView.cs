using Godot;
using System;

namespace DatePicker;

public partial class MonthView : VBoxContainer, ICalendarView
{

	[Export]
    public GridContainer Buttons { get; private set; }
    [Export]
	public Calendar Calendar { get; set; }

	DateTime[] dates;

	public override void _Ready()
	{
		dates = new DateTime[Buttons.GetChildCount()];
		for (int i = 0; i < Buttons.GetChildCount(); i++)
        {
            int index = i;
            Button button = Buttons.GetChild<Button>(index);
            button.Toggled += (toggled) => {
                if (!toggled)
                    return;
				Calendar.Selected = Calendar.DateTime = dates[index];
				Calendar.EmitSignal(Calendar.SignalName.Finished);
			};
        }
        Refresh();
	}

	public void Previous()
	{
		Calendar.DateTime = Calendar.DateTime.AddMonths(-1);
		Refresh();
	}

	public void Next()
	{
		Calendar.DateTime = Calendar.DateTime.AddMonths(1);
		Refresh();
	}

	void Refresh()
    {
        Calendar.Header.Text = Calendar.DateTime.ToString("MMMM yyyy");
		DateTime previous = Calendar.DateTime.AddMonths(-1);
        int prevDays = DateTime.DaysInMonth(previous.Year, previous.Month);
        int days = DateTime.DaysInMonth(Calendar.DateTime.Year, Calendar.DateTime.Month);
        DayOfWeek start = new DateTime(Calendar.DateTime.Year, Calendar.DateTime.Month, 1).DayOfWeek;
        for (int i = 0; i < (int) start; i++)
        {
			int day = prevDays - (int) start + i + 1;
            Button button = Buttons.GetChild<Button>(i);
			dates[button.GetIndex()] = new DateTime(previous.Year, previous.Month, day);
            button.Text = day.ToString();
            button.AddThemeColorOverride("font_color", new("828b98"));
            button.SetPressedNoSignal(day == Calendar.Selected.Day &&
                Calendar.Selected.Year == previous.Year &&
                Calendar.Selected.Month == previous.Month);
        }
        for (int i = 0; i < days; i++)
        {
            Button button = Buttons.GetChild<Button>(i + (int) start);
			dates[button.GetIndex()] = new DateTime(Calendar.DateTime.Year, Calendar.DateTime.Month, i + 1);
            button.Text = (i + 1).ToString();
            button.AddThemeColorOverride("font_color", new("222323"));
            button.SetPressedNoSignal(i + 1 == Calendar.Selected.Day &&
                Calendar.Selected.Year == Calendar.DateTime.Year &&
                Calendar.Selected.Month == Calendar.DateTime.Month);
        }
		DateTime next = Calendar.DateTime.AddMonths(1);
        for (int i = days + (int) start; i < Buttons.GetChildCount(); i++)
        {
			int day = i + 1 - days - (int) start;
            Button button = Buttons.GetChild<Button>(i);
			dates[button.GetIndex()] = new DateTime(next.Year, next.Month, day);
            button.Text = day.ToString();
            button.AddThemeColorOverride("font_color", new("828b98"));
            button.SetPressedNoSignal(day == Calendar.Selected.Day &&
                Calendar.Selected.Year == next.Year &&
                Calendar.Selected.Month == next.Month);
        }
   }
}
