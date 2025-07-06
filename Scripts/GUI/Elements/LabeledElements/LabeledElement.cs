using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An element with a label.
/// </summary>
public partial class LabeledElement : Element
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

    /* Public methods. */
    public override LabeledElement Copy()
    {
        LabeledElement copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public void CopyFrom(LabeledElement element)
    {
        CopyFrom(element as Element);
        LabelWidth = element.LabelWidth;
        LabelText = element.LabelText;
        LabelColor = element.LabelColor;
        LabelFontSize = element.LabelFontSize;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        base._Process(delta);

        // Set label width.
        Label.CustomMinimumSize = new(LabelWidth - GlobalIndentation, 0f);

        // Hide label if it is empty.
        Label.Visible = Label.Text != "";
    }

    /* Protected methods. */
    protected override void Initialize()
    {
        base.Initialize();

        // Create label.
        Label = new();
        Label.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        AddChild(Label);
    }
}