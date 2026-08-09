using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A LineEdit that suppresses built-in undo/redo behavior.
/// </summary>
public sealed partial class LineEdit : Godot.LineEdit
{
    /* Private properties. */
    private string OldValue { get; set; } = "";

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        if (!HasFocus())
            OldValue = Text;
    }

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
                Text = OldValue;
                ReleaseFocus();
                AcceptEvent();
            }
        }
    }
}