using Godot;

namespace Rusty.ISA.Editor;

public partial class ConsoleLine : MarginContainer
{
    /* Public methods. */
    public string LabelText { get; set; } = "";
    public Color LabelColor
    {
        get => Foldout.LabelColor;
        set => Foldout.LabelColor = value;
    }
    public Font LabelFont
    {
        get => Foldout.LabelFont;
        set => Foldout.LabelFont = value;
    }
    public Color BackgroundColor
    {
        get => Background.Color;
        set => Background.Color = value;
    }

    /* Private properties. */
    private ColorRect Background { get; set; }
    private Foldout Foldout { get; set; }

    /* Constructors. */
    public ConsoleLine()
    {
        Background = new();
        AddChild(Background);

        MarginContainer contents = new();
        contents.AddThemeConstantOverride("margin_left", 4);
        contents.AddThemeConstantOverride("margin_right", 4);
        contents.AddThemeConstantOverride("margin_bottom", 4);
        contents.AddThemeConstantOverride("margin_top", 4);
        AddChild(contents);

        Foldout = new();
        Foldout.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        Foldout.SizeFlagsVertical = SizeFlags.ExpandFill;
        contents.AddChild(Foldout);
    }

    /* Public methods. */
    public void Collapse()
    {
        Foldout.IsOpen = false;
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        Foldout.SizeFlagsHorizontal = Foldout.IsOpen ? SizeFlags.ShrinkBegin : SizeFlags.ExpandFill;
        if (Foldout.IsOpen)
            Foldout.LabelText = LabelText;
        else
        {
            int linebreakIndex = LabelText.IndexOf('\n');
            if (linebreakIndex != -1)
                Foldout.LabelText = LabelText.Substring(0, linebreakIndex);
            else
                Foldout.LabelText = LabelText;
        }

        base._Process(delta);
    }
}