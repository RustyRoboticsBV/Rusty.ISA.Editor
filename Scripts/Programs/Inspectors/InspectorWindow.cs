using Godot;

namespace Rusty.ISA.Editor;

public partial class InspectorWindow : MarginContainer
{
    /* Private properties. */
    private VBoxContainer Contents { get; set; }

    /* Constructors. */
    public InspectorWindow()
    {
        // Add background.
        ColorRect bg = new();
        bg.Color = new Color(0.3f, 0.3f, 0.3f);
        AddChild(bg);

        // Add scroll view.
        ScrollContainer scroll = new();
        AddChild(scroll);

        // Add margin around contents.
        MarginContainer contentsMargin = new();
        contentsMargin.AddThemeConstantOverride("margin_left", 4);
        contentsMargin.AddThemeConstantOverride("margin_right", 4);
        contentsMargin.AddThemeConstantOverride("margin_bottom", 4);
        contentsMargin.AddThemeConstantOverride("margin_top", 4);
        contentsMargin.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        contentsMargin.SizeFlagsVertical = SizeFlags.ExpandFill;
        scroll.AddChild(contentsMargin);

        // Add contents.
        Contents = new();
        contentsMargin.AddChild(Contents);

        // Add title elements.
        Label title = new();
        title.Text = "Inspector";
        title.HorizontalAlignment = HorizontalAlignment.Center;
        title.AddThemeFontSizeOverride("font_size", 20);
        Contents.AddChild(title);
        title.Name = "Title";

        HSeparator separator = new();
        Contents.AddChild(separator);
        separator.Name = "Separator";
    }

    /* Public methods. */
    public void Add(Control inspector) 
    {
        Contents.AddChild(inspector);
    }

    public void Remove(Control inspector)
    {
        Contents.RemoveChild(inspector);
    }

    /* Godot overrides. */
    public override void _Ready()
    {
        // Set minimum width.
        CustomMinimumSize = new(GetViewportRect().Size.X * 0.25f, 0);
    }
}
