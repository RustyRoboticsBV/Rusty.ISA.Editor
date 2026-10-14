using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A HSlider that suppresses built-in undo/redo behavior.
/// </summary>
internal sealed partial class HSlider : Godot.HSlider
{
    /* Public properties. */
    public UndoRedo UndoRedo { get; set; }

    /* Private properties. */
    private double LastValue { get; set; }

    /* Public events. */
    public event Action<double> CommittedValue;

    /* Constructors. */
    public HSlider() : base()
    {
        Scrollable = false;
        DragStarted += OnDragStarted;
        DragEnded += OnDragEnded;
    }

    /* Public methods. */
    public new void SetValue(double value)
    {
        Value = value;
        LastValue = value;
    }

    public void CommitValue(double value)
    {
        if (value != LastValue)
        {
            Record(LastValue, value);
            Value = value;
            LastValue = value;
            CommittedValue?.Invoke(value);
        }
    }

    public void CancelValue()
    {
        Value = LastValue;
        ReleaseFocus();
    }

    /* Godot overrides. */
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventKey key && key.Pressed)
        {
            if (key.CtrlPressed && key.Keycode == Key.Z)
                AcceptEvent();
            else if (key.CtrlPressed && key.Keycode == Key.Y)
                AcceptEvent();
            else if (!key.CtrlPressed && key.Keycode == Key.Escape)
            {
                CancelValue();
                AcceptEvent();
            }
            else if (!key.CtrlPressed && key.Keycode == Key.Enter)
            {
                ReleaseFocus();
                AcceptEvent();
            }
        }
    }

    /* Private methods. */
    private void OnDragStarted()
    {
        LastValue = Value;
    }

    private void OnDragEnded(bool valueChanged)
    {
        CommitValue(Value);
    }

    private void Record(double from, double to)
    {
        if (UndoRedo == null || from == to)
            return;

        UndoRedo.CreateAction($"Changed HSlider '{Name}': {from} => {to}");

        UndoRedo.AddUndoProperty(this, "value", from);
        UndoRedo.AddUndoProperty(this, nameof(LastValue), from);

        UndoRedo.AddDoProperty(this, "value", to);
        UndoRedo.AddDoProperty(this, nameof(LastValue), to);

        UndoRedo.CommitAction(false);
    }
}