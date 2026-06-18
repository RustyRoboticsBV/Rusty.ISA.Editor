using Godot;

namespace Rusty.ISA.Console;

/// <summary>
/// An element with a label.
/// </summary>
public partial class LabeledElement : HBoxContainer
{
    /* Public properties. */
    public int LabelWidth { get; set; }
    public string LabelText
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    public Color LabelColor
    {
        get => Label.Modulate;
        set => Label.Modulate = value;
    }
    public int LabelFontSize
    {
        get => Label.GetThemeFontSize("font_size");
        set => Label.AddThemeFontSizeOverride("font_size", value);
    }
    public Font LabelFont
    {
        get => Label.GetThemeFont("font");
        set => Label.AddThemeFontOverride("font", value);
    }
    public virtual new string TooltipText
    {
        get => base.TooltipText;
        set
        {
            base.TooltipText = value;
            Label.TooltipText = value;
        }
    }

    /* Protected properties. */
    protected Label Label { get; private set; }

    /* Constructors. */
    public LabeledElement()
    {
        // Create label.
        Label = new();
        Label.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        AddChild(Label);
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        // Set label width.
        Label.CustomMinimumSize = new(LabelWidth, 0f);

        // Hide label if it is empty.
        Label.Visible = Label.Text != "";
    }
}