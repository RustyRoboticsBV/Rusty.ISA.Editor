using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A ColorPickerButton that suppresses built-in undo/redo behavior.
/// </summary>
internal sealed partial class ColorPickerButton : Godot.ColorPickerButton
{
    /* Public properties. */
    public UndoRedo UndoRedo { get; set; }

    /* Private properties. */
    private Color LastColor { get; set; }

    /* Public events. */
    public event Action<Color> CommittedColor;

    /* Constructors. */
    public ColorPickerButton() : base()
    {
        CustomMinimumSize = new(0f, 20f);
        FocusExited += OnFocusExited;
        GetPopup().PopupHide += OnPopupHide;
    }

    /* Public methods. */
    public void SetColor(Color color)
    {
        Color = color;
        LastColor = color;
    }

    public void CommitColor(Color color)
    {
        if (color != LastColor)
        {
            Record(LastColor, color);
            Color = color;
            LastColor = color;
            CommittedColor?.Invoke(color);
        }
    }

    public void CancelColor()
    {
        Color = LastColor;
        GetPopup().Hide();
        ReleaseFocus();
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        if (!GetPopup().Visible)
            LastColor = Color;
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
                    CancelColor();
                    AcceptEvent();
                }
            }
            else if (!key.CtrlPressed && key.Keycode == Key.Escape)
            {
                CancelColor();
                AcceptEvent();
            }
        }
    }

    /* Private methods. */
    private void OnFocusExited()
    {
        if (GetPopup().Visible)
            GetPopup().Hide();
        CommitColor(Color);
    }

    private void OnPopupHide()
    {
        if (HasFocus())
            ReleaseFocus();
        CommitColor(Color);
    }

    private void Record(Color from, Color to)
    {
        if (UndoRedo == null || from == to)
            return;

        UndoRedo.CreateAction($"Changed ColorPickerButton '{Name}': {from} => {to}");

        UndoRedo.AddUndoProperty(this, "color", from);
        UndoRedo.AddUndoProperty(this, nameof(LastColor), from);

        UndoRedo.AddDoProperty(this, "color", to);
        UndoRedo.AddDoProperty(this, nameof(LastColor), to);

        UndoRedo.CommitAction(false);
    }
}