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
			GetTree().Root.CallDeferred(Node.MethodName.AddChild, Calendar);
		}
		Calendar.Visible = false;
		Calendar.CalendarButton = this;
		Calendar.Finished += () => {
			Text = Calendar.Selected.ToString("M/d/yyyy");
			Calendar.Visible = false;
		};
		Pressed += () => Calendar.Visible = !Calendar.Visible;
		TreeExited += Calendar.QueueFree;
	}
}
