using Godot;
using System.Globalization;

namespace DatePicker;

/// <summary>
/// Displays the months of a year.
/// </summary>
public partial class YearView : GridContainer, ICalendarView
{

	/// <summary>
	/// The calendar that this view is a part of.
	/// </summary>
	[Export]
	public Calendar Calendar { get; set; }

	public override void _Ready()
	{
		DateTimeFormatInfo dtf = new CultureInfo("en-US", false).DateTimeFormat;
		for (int i = 0; i < dtf.AbbreviatedMonthNames.Length; i++)
		{
			string name = dtf.AbbreviatedMonthNames[i];
			if (name.Length == 0)
				continue;
			int month = i + 1;
			Button button = GetChild<Button>(i);
			button.Text = name;
			button.Pressed += () => {
				Calendar.Selected = Calendar.DateTime = new(Calendar.DateTime.Year, month, Calendar.DateTime.Day);
				Calendar.SetView(ResourceLoader.Load<PackedScene>("res://addons/DatePicker/MonthView.tscn").Instantiate<MonthView>());
			};
		}
		Refresh();
	}

	public void Previous()
	{
		Calendar.DateTime = Calendar.DateTime.AddYears(-1);
		Refresh();
	}

	public void Next()
	{
		Calendar.DateTime = Calendar.DateTime.AddYears(1);
		Refresh();
	}

	public void Refresh()
	{
		Calendar.Header.Text = Calendar.DateTime.ToString("yyyy");
		for (int i = 0; i < GetChildCount(); i++)
		{
			Button button = GetChild<Button>(i);
			button.SetPressedNoSignal(Calendar.Selected.Month == i + 1 && Calendar.Selected.Year == Calendar.DateTime.Year);
		}
	}
}
