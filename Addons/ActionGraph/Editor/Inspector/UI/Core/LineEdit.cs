using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A LineEdit that suppresses built-in undo/redo behavior.
/// </summary>
public sealed partial class LineEdit : Godot.LineEdit
{
    /* Public properties. */
    public UndoRedo UndoRedo { get; set; }

    /* Private properties. */
    private string LastText { get; set; } = "";

    /* Constructors. */
    public LineEdit() : base()
    {
        FocusExited += OnFocusExited;
    }

    /* Public methods. */
    public void SetValue(string text)
    {
        Text = text;
        LastText = text;
    }

    public void CommitValue(string text)
    {
        if (text != LastText)
        {
            Record(LastText, Text);
            Text = text;
            LastText = text;
        }
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        if (!HasFocus())
            LastText = Text;
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
                Text = LastText;
                ReleaseFocus();
            }
        }
    }

    /* Private methods. */
    private void OnFocusExited()
    {
        GD.Print("Release " + Name);
        CommitValue(Text);
    }

    private void Record(string from, string to)
    {
        if (UndoRedo == null || from == to)
            return;
        GD.Print($"Changed LineEdit '{Name}': {from} => {to}");

        UndoRedo.CreateAction($"Changed LineEdit '{Name}': {from} => {to}");

        UndoRedo.AddUndoProperty(this, "text", from);

        UndoRedo.AddDoProperty(this, "text", to);

        UndoRedo.CommitAction(false);
    }
}