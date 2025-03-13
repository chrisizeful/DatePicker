using Godot;

namespace DatePicker;

public partial class DateButton : Button
{

	[Export]
	public Calendar Calendar { get; set; }

	public override void _Ready()
	{
		if (Calendar == null)
		{
			Calendar = ResourceLoader.Load<PackedScene>("res://addons/DatePicker/Calendar.tscn").Instantiate<Calendar>();
			Calendar.Visible = false;
			GetTree().Root.CallDeferred(Node.MethodName.AddChild, Calendar);
		}
		Calendar.DateSelected += () => {
			if (Calendar.View is not MonthView)
				return;
			Text = Calendar.Selected.ToString("M/d/yyyy");
			Calendar.Visible = false;
		};
		Calendar.CalendarButton = this;
		Pressed += () => {
			if (Calendar != null)
				Calendar.Visible = !Calendar.Visible;
		};
	}
}
