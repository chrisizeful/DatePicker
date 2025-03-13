using System;
using Godot;

namespace DatePicker;

public partial class Calendar : Control
{

    [Export]
    public Button Header { get; private set; }
    [Export]
    public BaseButton Next { get; private set; }
    [Export]
    public BaseButton Previous { get; private set; }
    [Export]
    public Control ViewParent { get; private set; }

    public float Margin { get; set; } = 16;
    public ICalendarView View => ViewParent.GetChild<ICalendarView>(0);

    DateTime dateTime = DateTime.Now;
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
    public DateTime Selected
    {
        get => selected;
        set
        {
            selected = value;
            EmitSignal(SignalName.DateSelected);
        }
    }

    public DateTime LowerLimit { get; set; }
    public DateTime UpperLimit { get; set; }
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

    [Signal]
    public delegate void ViewChangedEventHandler(Control view);
    [Signal]
	public delegate void DateChangedEventHandler();
    [Signal]
	public delegate void DateSelectedEventHandler();
    [Signal]
	public delegate void FinishedEventHandler();
}