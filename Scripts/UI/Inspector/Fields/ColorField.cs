using Godot;
using System;

namespace Rusty.ISA;

/// <summary>
/// A color field.
/// </summary>
public partial class ColorField : HBoxContainer, IWidget, IValued<Color>
{
    /* Public properties. */
    public string Title
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public int TitleWidth
    {
        get => (int)Label.CustomMinimumSize.X;
        set => Label.CustomMinimumSize = new(value, Label.CustomMinimumSize.Y);
    }
    public string Description
    {
        get => TooltipText;
        set
        {
            TooltipText = value;
            Label.TooltipText = value;
            ColorPickerButton.TooltipText = value;
        }
    }

    public Color Value
    {
        get => ColorPickerButton.Color;
        set => ColorPickerButton.Color = value;
    }

    /* Private methods. */
    private Label Label { get; set; }
    private ColorPickerButton ColorPickerButton { get; set; }

    /* Public events. */
    public event Action<IWidget> Changed;

    /* Constructors. */
    public ColorField()
    {
        Label = new();
        AddChild(Label);
        TitleWidth = 160;

        ColorPickerButton = new();
        AddChild(ColorPickerButton);
        ColorPickerButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        ColorPickerButton.ColorChanged += OnChanged;
    }

    /* Public methods. */
    public IWidget Copy()
    {
        ColorField field = new();
        field.SizeFlagsHorizontal = SizeFlagsHorizontal;
        field.SizeFlagsVertical = SizeFlagsVertical;
        field.Title = Title;
        field.TitleWidth = TitleWidth;
        field.Description = Description;
        field.Value = Value;
        return field;
    }

    public void ExpandFill(bool vertical = false)
    {
        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        if (vertical)
            SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    /* Private methods. */
    private void OnChanged(Color color)
    {
        Godot.GD.Print("FCUK COLOR");
        Changed?.Invoke(this);
    }
}