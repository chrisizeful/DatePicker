namespace DatePicker;

public interface ICalendarView
{

	public Calendar Calendar { get; set; }

    void Previous();
    void Next();
    void Refresh();
}