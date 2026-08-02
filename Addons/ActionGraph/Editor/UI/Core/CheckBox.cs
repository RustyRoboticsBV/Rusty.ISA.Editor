using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A CheckBox that suppresses built-in undo/redo behavior.
/// </summary>
public sealed partial class CheckBox : Godot.CheckBox
{
    /* Godot overrides. */
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventKey key && key.Pressed)
        {
            if (key.CtrlPressed && key.Keycode == Key.Z)
                AcceptEvent();
            if (key.CtrlPressed && key.Keycode == Key.Y)
                AcceptEvent();
        }
    }
}