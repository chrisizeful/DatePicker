namespace DatePicker;

/// <summary>
/// A view that dictates how a calendar should be displayed (i.e. <see cref="MonthView"/> or <see cref="YearView"/>).
/// </summary>
public interface ICalendarView
{

	/// <summary>
	/// The calendar that this view is a part of.
	/// </summary>
	public Calendar Calendar { get; set; }

    /// <summary>
    /// When the user clicks the previous button (i.e. go to the previous year).
    /// </summary>
    void Previous();
    /// <summary>
    /// When the user clicks the next button (i.e. go to the next year).
    /// </summary>
    void Next();
    /// <summary>
    /// Update the view to reflect the selected date.
    /// </summary>
    void Refresh();
}