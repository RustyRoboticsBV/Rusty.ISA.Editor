using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A CheckBox that suppresses built-in undo/redo behavior.
/// </summary>
public sealed partial class CheckBox : Godot.CheckBox
{
    /* Public properties. */
    public UndoRedo UndoRedo { get; set; }

    /* Constructors. */
    public CheckBox() : base()
    {
        Pressed += OnPressed;
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
            else if (!key.CtrlPressed && (key.Keycode == Key.Escape || key.Keycode == Key.Enter))
            {
                ReleaseFocus();
                AcceptEvent();
            }
        }
    }

    /* Private methods. */
    private void OnPressed()
    {
        Record(!ButtonPressed, ButtonPressed);
        ReleaseFocus();
    }

    private void Record(bool from, bool to)
    {
        if (UndoRedo == null || from == to)
            return;

        UndoRedo.CreateAction($"Changed CheckButton '{Name}': {from} => {to}");

        UndoRedo.AddUndoProperty(this, "button_pressed", from);

        UndoRedo.AddDoProperty(this, "button_pressed", to);

        UndoRedo.CommitAction(false);
    }
}