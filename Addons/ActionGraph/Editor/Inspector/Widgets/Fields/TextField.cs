using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A text area field.
/// </summary>
internal partial class TextField : VBoxContainer, IField<TextField>
{
    /* Public properties. */
    public string TitleText
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public int TitleWidth
    {
        get => (int)Label.CustomMinimumSize.X;
        set => Label.CustomMinimumSize = new(value, Label.CustomMinimumSize.Y);
    }
    public new string TooltipText
    {
        get => base.TooltipText;
        set
        {
            base.TooltipText = value;
            Label.TooltipText = value;
            TextEdit.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo
    {
        get => TextEdit.UndoRedo;
        set => TextEdit.UndoRedo = value;
    }

    public string Text => TextEdit.Text;

    /* Private methods. */
    private Label Label { get; set; }
    private TextEdit TextEdit { get; set; }

    /* Public events. */
    public event Action<TextField> TextChanged;

    /* Constructors. */
    public TextField()
    {
        Label = new();
        Label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Label, false, InternalMode.Front);
        TitleWidth = 160;

        TextEdit = new();
        TextEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        TextEdit.CustomMinimumSize = new(0f, 100f);
        TextEdit.TextChanged += () => TextChanged?.Invoke(this);
        AddChild(TextEdit, false, InternalMode.Front);
    }

    public TextField(string text) : this() => SetText(text);

    public TextField(string title, string text) : this(text) => TitleText = title;

    /* Public methods. */
    public TextField DuplicateWidget()
    {
        TextField field = Duplicate() as TextField;
        field.TitleText = TitleText;
        field.TitleWidth = TitleWidth;
        field.TooltipText = TooltipText;
        field.SetText(Text);
        return field;
    }

    /// <summary>
    /// Change the text without recording it in undo/redo.
    /// </summary>
    public void SetText(string text) => TextEdit.SetText(text);
    /// <summary>
    /// Change the text and record it in undo/redo.
    /// </summary>
    public void CommitText(string text) => TextEdit.CommitText(text);
    /// <summary>
    /// If an edit is in progress, cancel it and revert to the last committed color.
    /// </summary>
    public void CancelText() => TextEdit.CancelText();
}