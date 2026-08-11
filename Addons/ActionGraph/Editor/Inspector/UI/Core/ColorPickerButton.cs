using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A ColorPickerButton that suppresses built-in undo/redo behavior.
/// </summary>
public sealed partial class ColorPickerButton : Godot.ColorPickerButton
{
    /* Public properties. */
    public UndoRedo UndoRedo { get; set; }

    /* Private properties. */
    private Color LastColor { get; set; }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        CustomMinimumSize = new(0, 20);
        GetPopup().PopupHide += OnPopupHide;
    }

    public override void _Process(double delta)
    {
        if (!GetPopup().Visible)
            LastColor = Color;
    }

    /* Public methods. */
    public void SetValue(Color color)
    {
        Color = color;
        LastColor = color;
    }

    public void CommitValue(Color color)
    {
        if (color != LastColor)
        {
            Record(LastColor, Color);
            Color = color;
            LastColor = color;
        }
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
                    Color = LastColor;
                    GetPopup().Hide();
                    ReleaseFocus();
                    AcceptEvent();
                }
            }
            else if (!key.CtrlPressed && key.Keycode == Key.Escape)
            {
                Color = LastColor;
                GetPopup().Hide();
                ReleaseFocus();
                AcceptEvent();
            }
        }
    }

    /* Private methods. */
    private void OnFocusExited()
    {
        if (GetPopup().Visible)
            GetPopup().Hide();
        CommitValue(Color);
    }

    private void OnPopupHide()
    {
        if (HasFocus())
            ReleaseFocus();
        CommitValue(Color);
    }

    private void Record(Color from, Color to)
    {
        if (UndoRedo == null || from == to)
            return;

        UndoRedo.CreateAction($"Changed ColorPickerButton '{Name}': {from} => {to}");

        UndoRedo.AddUndoProperty(this, "color", from);

        UndoRedo.AddDoProperty(this, "color", to);

        UndoRedo.CommitAction(false);
    }
}