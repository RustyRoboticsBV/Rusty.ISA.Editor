using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A boolean button field.
/// </summary>
internal partial class BoolField : HBoxContainer, IField<BoolField>
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
            CheckButton.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo
    {
        get => CheckButton.UndoRedo;
        set => CheckButton.UndoRedo = value;
    }

    public bool Pressed
    {
        get => CheckButton.ButtonPressed;
        set => CheckButton.ButtonPressed = value;
    }

    /* Private methods. */
    private Label Label { get; set; }
    private CheckButton CheckButton { get; set; }

    /* Public events. */
    public event Action<BoolField> Toggled;

    /* Constructors. */
    public BoolField()
    {
        Label = new();
        AddChild(Label, false, InternalMode.Front);
        TitleWidth = 160;

        CheckButton = new();
        CheckButton.Pressed += () => Toggled?.Invoke(this);
        AddChild(CheckButton, false, InternalMode.Front);
    }

    public BoolField(bool pressed) : this() => SetPressed(pressed);

    public BoolField(string title, bool pressed) : this(pressed) => TitleText = title;

    /* Public methods. */
    public BoolField DuplicateField()
    {
        BoolField field = Duplicate() as BoolField;
        field.TitleText = TitleText;
        field.TitleWidth = TitleWidth;
        field.TooltipText = TooltipText;
        field.UndoRedo = UndoRedo;
        field.SetPressed(Pressed);
        return field;
    }

    /// <summary>
    /// Change the value without recording it in undo/redo.
    /// </summary>
    public void SetPressed(bool @true) => CheckButton.SetPressed(@true);
    /// <summary>
    /// Change the value and record it in undo/redo.
    /// </summary>
    public void CommitPressed(bool @true) => CheckButton.CommitPressed(@true);
}