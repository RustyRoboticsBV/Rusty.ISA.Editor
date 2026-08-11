using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A TextEdit that suppresses built-in undo/redo behavior.
/// </summary>
public sealed partial class TextEdit : Godot.TextEdit
{
    /* Public properties. */
    public UndoRedo UndoRedo { get; set; }

    /* Private properties. */
    private string LastText { get; set; } = "";

    /* Public events. */
    public event Action<string> CommittedText;

    /* Constructors. */
    public TextEdit() : base()
    {
        CustomMinimumSize = new(0f, 100f);
        FocusEntered += OnFocusEntered;
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
        }
    }

    /* Private methods. */
    private void OnFocusEntered()
    {
        LastText = Text;
    }

    private void OnFocusExited()
    {
        CommitText(Text);
    }

    private void Record(string from, string to)
    {
        if (UndoRedo == null || from == to)
            return;

        UndoRedo.CreateAction($"Changed TextEdit '{Name}': {from} => {to}");

        UndoRedo.AddUndoProperty(this, "text", from);

        UndoRedo.AddDoProperty(this, "text", to);

        UndoRedo.CommitAction(false);
    }
}