using System;
using Godot;

namespace DatePicker;

/// <summary>
/// A calendar that can be used to select a date from a set view.
/// </summary>
public partial class Calendar : Control
{

    /// <summary>
    /// The header button/text (i.e. "Month" or "Year").
    /// </summary>
    [Export]
    public Button Header { get; private set; }
    /// <summary>
    /// The button to go the next part of the view (i.e. next month/year).
    /// </summary>
    [Export]
    public BaseButton Next { get; private set; }
    /// <summary>
    /// The button to go the previous part of the view (i.e. next month/year).
    /// </summary>
    [Export]
    public BaseButton Previous { get; private set; }
    /// <summary>
    /// The parent of the ICalendarView.
    /// </summary>
    [Export]
    public Control ViewParent { get; private set; }

    /// <summary>
    /// The vertical space between the calendar and the button that opens it.
    /// </summary>
    public float Margin { get; set; } = 16;
    /// <summary>
    /// The ICalendarView currently being displayed.
    /// </summary>
    public ICalendarView View => ViewParent.GetChild<ICalendarView>(0);

    DateTime dateTime = DateTime.Now;
    /// <summary>
    /// The date that the calendar is currently displaying.
    /// </summary>
    public DateTime DateTime
    {
        get => dateTime;
        set
        {
            dateTime = value;
            EmitSignal(SignalName.DateChanged);
        }
    }

    DateTime selected = DateTime.Now;
    /// <summary>
    /// The date that the user has selected.
    /// </summary>
    public DateTime Selected
    {
        get => selected;
        set
        {
            selected = value;
            EmitSignal(SignalName.DateSelected);
        }
    }

    /// <summary>
    /// The earliest date that can be selected.
    /// NOTE: Up to the user to implement the functionality.
    /// </summary>
    public DateTime LowerLimit { get; set; }
    /// <summary>
    /// The latest date that can be selected.
    /// NOTE: Up to the user to implement the functionality.
    /// </summary>
    public DateTime UpperLimit { get; set; }
    /// <summary>
    /// The button that displays this calendar - can be null.
    /// </summary>
    public Button CalendarButton { get; set; }

    public override void _Ready()
    {
        Header.Pressed += () => {
            string path = View is MonthView ? "res://addons/DatePicker/YearView.tscn" : "res://addons/DatePicker/MonthView.tscn";
            SetView(ResourceLoader.Load<PackedScene>(path).Instantiate<ICalendarView>());
        };
        Previous.Pressed += () => View.Previous();
        Next.Pressed += () => View.Next();
    }

    public override void _EnterTree()
    {
        GetTree().Root.GuiFocusChanged += OnGuiFocusChanged;
    }

    public override void _ExitTree()
    {
        GetTree().Root.GuiFocusChanged -= OnGuiFocusChanged;
    }

    /// <summary>
    /// When the focus changes, hide the calendar if the new focus is not a child of the calendar.
    /// </summary>
    /// <param name="focus"></param>
    void OnGuiFocusChanged(Node focus)
    {
        if (!IsAncestorOf(focus) && focus is not DateButton)
            Visible = false;
    }

    public override void _Process(double delta)
    {
        if (CalendarButton == null)
            return;
        Vector2 position = CalendarButton.GlobalPosition;
        position.X += (CalendarButton.Size.X - Size.X) / 2.0f;
        position.Y += CalendarButton.Size.Y + Margin;
        GlobalPosition = position;
    }

    /// <summary>
    /// Change the view.
    /// </summary>
    /// <param name="view">The new view.</param>
    public void SetView(ICalendarView view)
    {
        ((Control) View).QueueFree();
        view.Calendar = this;
        Control control = (Control) view;
        ViewParent.AddChild(control);
        EmitSignal(SignalName.ViewChanged, control);
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb && mb.Pressed)
        {
            if (mb.ButtonIndex == MouseButton.WheelDown) View.Next();
            else if (mb.ButtonIndex == MouseButton.WheelUp) View.Previous();
        }
    }

    /// <summary>
    /// The signal emitted when the view changes.
    /// </summary>
    /// <param name="view"></param>
    [Signal]
    public delegate void ViewChangedEventHandler(Control view);
    /// <summary>
    /// The signal emitted when the date changes.
    /// </summary>
    [Signal]
	public delegate void DateChangedEventHandler();
    /// <summary>
    /// The signal emitted when the user selects a date in ANY view. Emitted when selecting
    /// both a month and a day.
    /// </summary>
    [Signal]
	public delegate void DateSelectedEventHandler();
    /// <summary>
    /// The signal emitted when the user is finished selecting a date. Meaning, the user
    /// has selected a date in EVERY view (i.e. a month and a day).
    /// </summary>
    [Signal]
	public delegate void FinishedEventHandler();
}