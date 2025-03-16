using Godot;
using System;

namespace DatePicker;

/// <summary>
/// Displays the days in a month.
/// </summary>
public partial class MonthView : VBoxContainer, ICalendarView
{

    /// <summary>
    /// The buttons that represent the days in a month.
    /// </summary>
	[Export]
    public GridContainer Buttons { get; private set; }
    /// <summary>
	/// The calendar that this view is a part of.
	/// </summary>
    [Export]
	public Calendar Calendar { get; set; }

    /// <summary>
    /// One date per button. The index of the button corresponds to the index of the date.
    /// </summary>
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

    /// <summary>
    /// When the user clicks the previous button (i.e. go to the previous year).
    /// </summary>
	public void Previous()
	{
		Calendar.DateTime = Calendar.DateTime.AddMonths(-1);
		Refresh();
	}

    /// <summary>
    /// When the user clicks the next button (i.e. go to the next year).
    /// </summary>
	public void Next()
	{
		Calendar.DateTime = Calendar.DateTime.AddMonths(1);
		Refresh();
	}

    /// <summary>
    /// Update the view to reflect the selected date.
    /// </summary>
	public void Refresh()
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
