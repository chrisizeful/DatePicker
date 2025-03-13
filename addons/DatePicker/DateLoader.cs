#if TOOLS
using Godot;

namespace DatePicker;

/// <summary>
/// Plugin that add/removes the <see cref="DatePicker"/> and <see cref="Calendar"/> nodes.
/// </summary>
[Tool]
public partial class DateLoader : EditorPlugin
{

	public override void _EnablePlugin()
	{
		AddCustomType("DateButton", "Button", ResourceLoader.Load<CSharpScript>("res://addons/DatePicker/DateButton.cs"), null);
		AddCustomType("Calendar", "Control", ResourceLoader.Load<CSharpScript>("res://addons/DatePicker/Calendar.cs"), null);
	}

	public override void _DisablePlugin()
	{
		RemoveCustomType("DatePicker");
		RemoveCustomType("Calendar");
	}
}
#endif
