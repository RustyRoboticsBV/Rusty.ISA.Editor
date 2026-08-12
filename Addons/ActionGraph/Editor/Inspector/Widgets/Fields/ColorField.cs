using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A color field.
/// </summary>
internal partial class ColorField : HBoxContainer, IField<ColorField>
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
            ColorPickerButton.TooltipText = value;
        }
    }
    public UndoRedo UndoRedo
    {
        get => ColorPickerButton.UndoRedo;
        set => ColorPickerButton.UndoRedo = value;
    }

    public Color Color => ColorPickerButton.Color;

    /* Private methods. */
    private Label Label { get; set; }
    private ColorPickerButton ColorPickerButton { get; set; }

    /* Public events. */
    public event Action<ColorField> ColorChanged;

    /* Constructors. */
    public ColorField()
    {
        Label = new();
        AddChild(Label, false, InternalMode.Front);
        TitleWidth = 160;

        ColorPickerButton = new();
        ColorPickerButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        ColorPickerButton.CustomMinimumSize = new(0f, 31f);
        ColorPickerButton.Color = Colors.White;
        ColorPickerButton.ColorChanged += (color) => ColorChanged?.Invoke(this);
        AddChild(ColorPickerButton, false, InternalMode.Front);
    }

    public ColorField(Color color) : this() => ColorPickerButton.Color = color;

    public ColorField(string title, Color color) : this(color) => TitleText = title;

    /* Public methods. */
    public ColorField DuplicateField()
    {
        ColorField field = Duplicate() as ColorField;
        field.TitleText = TitleText;
        field.TitleWidth = TitleWidth;
        field.TooltipText = TooltipText;
        field.UndoRedo = UndoRedo;
        field.ColorPickerButton.Color = Color;
        return field;
    }

    /// <summary>
    /// Change the color without recording it in undo/redo.
    /// </summary>
    public void SetColor(Color color) => ColorPickerButton.SetColor(color);
    /// <summary>
    /// Change the color and record it in undo/redo.
    /// </summary>
    public void CommitColor(Color color) => ColorPickerButton.CommitColor(color);
    /// <summary>
    /// If an edit is in progress, cancel it and revert to the last committed color.
    /// </summary>
    public void CancelColor() => ColorPickerButton.CancelColor();
}