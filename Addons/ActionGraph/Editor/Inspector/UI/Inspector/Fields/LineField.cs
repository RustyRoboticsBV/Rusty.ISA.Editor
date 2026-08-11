using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A text line field.
/// </summary>
public partial class LineField : HBoxContainer, IField<LineField>
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
            LineEdit.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo
    {
        get => LineEdit.UndoRedo;
        set => LineEdit.UndoRedo = value;
    }

    public string Text => LineEdit.Text;

    /* Private methods. */
    private Label Label { get; set; }
    private LineEdit LineEdit { get; set; }

    /* Public events. */
    public event Action<LineField> TextChanged;

    /* Constructors. */
    public LineField()
    {
        Label = new();
        AddChild(Label, false, InternalMode.Front);
        TitleWidth = 160;

        LineEdit = new();
        LineEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        LineEdit.TextChanged += (text) => TextChanged?.Invoke(this);
        AddChild(LineEdit, false, InternalMode.Front);
    }

    public LineField(string text) : this() => SetText(text);

    public LineField(string title, string text) : this(text) => TitleText = title;

    /* Public methods. */
    public LineField DuplicateField()
    {
        LineField field = Duplicate() as LineField;
        field.TooltipText = TooltipText;
        field.TitleWidth = TitleWidth;
        field.TooltipText = TooltipText;
        field.SetText(Text);
        return field;
    }

    /// <summary>
    /// Change the text without recording it in undo/redo.
    /// </summary>
    public void SetText(string text) => LineEdit.SetText(text);
    /// <summary>
    /// Change the text and record it in undo/redo.
    /// </summary>
    public void CommitText(string text) => LineEdit.CommitText(text);
    /// <summary>
    /// If an edit is in progress, cancel it and revert to the last committed color.
    /// </summary>
    public void CancelText() => LineEdit.CancelText();
}