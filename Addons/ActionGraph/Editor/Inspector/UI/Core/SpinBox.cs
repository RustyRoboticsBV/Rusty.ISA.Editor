using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A SpinBox that suppresses built-in undo/redo behavior.
/// </summary>
public sealed partial class SpinBox : Godot.SpinBox
{
    /* Private properties. */
    private double OldValue { get; set; }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        if (!HasFocus())
            OldValue = Value;
    }

    public override void _GuiInput(InputEvent @event)
    {
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
                    Value = OldValue;
                    GetLineEdit().ReleaseFocus();
                    ReleaseFocus();
                    AcceptEvent();
                }
            }
            else if (!key.CtrlPressed && key.Keycode == Key.Escape)
            {
                Value = OldValue;
                GetLineEdit().ReleaseFocus();
                ReleaseFocus();
                AcceptEvent();
            }
        }
    }
}