using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An element with a label.
/// </summary>
public partial class LabeledElement : HBoxContainer, IGuiElement
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

    /* Protected properties. */
    protected Label Label { get; private set; }

    /* COnstructors. */
    public LabeledElement()
    {
        // Create label.
        Label = new();
        Label.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        AddChild(Label);
    }

    /* Public methods. */
    public virtual IGuiElement Copy()
    {
        LabeledElement copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public virtual void CopyFrom(IGuiElement other)
    {
        if (other is LabeledElement label)
        {
            LabelWidth = label.LabelWidth;
            LabelText = label.LabelText;
            LabelColor = label.LabelColor;
            LabelFontSize = label.LabelFontSize;
        }
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