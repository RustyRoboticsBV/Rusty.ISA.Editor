using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A SpinBox that suppresses built-in undo/redo behavior.
/// </summary>
public sealed partial class SpinBox : Godot.SpinBox
{
    /* Public properties. */
    public UndoRedo UndoRedo { get; set; }

    /* Private properties. */
    private double LastValue { get; set; }

    /* Public events. */
    public event Action<double> CommittedValue;

    /* Constructors. */
    public SpinBox() : base()
    {
        GetLineEdit().FocusEntered += OnFocusEntered;
        FocusEntered += OnFocusEntered;

        GetLineEdit().FocusExited += OnFocusExited;
        FocusExited += OnFocusExited;
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
        GetLineEdit().Text = LastValue.ToString();
        GetLineEdit().ReleaseFocus();
        ReleaseFocus();
    }

    /* Godot overrides. */
    public override void _Input(InputEvent @event)
    {
        if (!GetLineEdit().HasFocus())
            return;

        if (@event is InputEventKey key)
        {
            if (key.Pressed)
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
                    GetLineEdit().ReleaseFocus();
                    ReleaseFocus();
                    AcceptEvent();
                }
            }
        }
    }

    /* Private methods. */
    private void OnFocusEntered()
    {
        LastValue = Value;
    }

    private void OnFocusExited()
    {
        if (double.TryParse(GetLineEdit().Text, out double parsed))
            CommitValue(parsed);
        LastValue = Value;
    }

    private void Record(double from, double to)
    {
        if (UndoRedo == null || from == to)
            return;

        UndoRedo.CreateAction($"Changed SpinBox '{Name}': {from} => {to}");

        UndoRedo.AddUndoProperty(this, "value", from);

        UndoRedo.AddDoProperty(this, "value", to);

        UndoRedo.CommitAction(false);
    }
}