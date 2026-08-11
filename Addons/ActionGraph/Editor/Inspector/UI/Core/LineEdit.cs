using Godot;
using System;

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

    /* Public events. */
    public event Action<string> CommittedText;

    /* Constructors. */
    public LineEdit() : base()
    {
        FocusExited += OnFocusExited;
    }

    /* Public methods. */
    public new void SetText(string text)
    {
        Text = text;
        LastText = text;
    }

    public void CommitText(string text)
    {
        if (text != LastText)
        {
            Record(LastText, text);
            Text = text;
            LastText = text;
            CommittedText?.Invoke(text);
        }
    }

    public void CancelText()
    {
        Text = LastText;
        ReleaseFocus();
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
                CancelText();
                AcceptEvent();
            }
            else if (!key.CtrlPressed && key.Keycode == Key.Enter)
            {
                CancelText();
                AcceptEvent();
            }
        }
    }

    /* Private methods. */
    private void OnFocusExited()
    {
        CommitText(Text);
    }

    private void Record(string from, string to)
    {
        if (UndoRedo == null || from == to)
            return;

        UndoRedo.CreateAction($"Changed LineEdit '{Name}': {from} => {to}");

        UndoRedo.AddUndoProperty(this, "text", from);
        UndoRedo.AddUndoProperty(this, nameof(LastText), from);

        UndoRedo.AddDoProperty(this, "text", to);
        UndoRedo.AddDoProperty(this, nameof(LastText), to);

        UndoRedo.CommitAction(false);
    }
}