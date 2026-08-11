using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A FoldableHeader that suppresses built-in undo/redo behavior.
/// </summary>
public sealed partial class FoldableHeader : LinkButton
{
    /* Public properties. */
    public UndoRedo UndoRedo { get; set; }
    public new string Text { get; set; }
    public bool IsOpen { get; private set; } = true;

    /* Constructors. */
    public FoldableHeader() : base()
    {
        Underline = UnderlineMode.Never;
        Pressed += OnPressed;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        if (IsOpen)
            base.Text = "\u25BC " + Text;
        else
            base.Text = "\u25B6 " + Text;
    }

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
        Record(!IsOpen, IsOpen);
        IsOpen = !IsOpen;
        ReleaseFocus();
    }

    private void Record(bool from, bool to)
    {
        if (UndoRedo == null || from == to)
            return;

        UndoRedo.CreateAction($"Changed FoldableHeader '{Name}': {from} => {to}");

        UndoRedo.AddUndoProperty(this, "button_pressed", from);

        UndoRedo.AddDoProperty(this, "button_pressed", to);

        UndoRedo.CommitAction(false);
    }
}