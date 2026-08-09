using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A ColorPickerButton that suppresses built-in undo/redo behavior.
/// </summary>
public sealed partial class ColorPickerButton : Godot.ColorPickerButton
{
    /* Private properties. */
    private Color OldValue { get; set; }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        CustomMinimumSize = new(0, 20);
    }

    public override void _Process(double delta)
    {
        if (!GetPopup().Visible)
            OldValue = Color;
    }

    /* Godot overrides. */
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
                    Color = OldValue;
                    GetPopup().Hide();
                    ReleaseFocus();
                    AcceptEvent();
                }
            }
            else if (!key.CtrlPressed && key.Keycode == Key.Escape)
            {
                Color = OldValue;
                GetPopup().Hide();
                ReleaseFocus();
                AcceptEvent();
            }
        }
    }
}