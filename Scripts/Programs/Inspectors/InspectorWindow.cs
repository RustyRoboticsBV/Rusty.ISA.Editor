using Godot;

namespace Rusty.ISA.Editor;

public partial class InspectorWindow : MarginContainer
{
    /* Private properties. */
    private VBoxContainer Contents { get; set; }

    /* Constructors. */
    public InspectorWindow()
    {
        // Set margins.
        AddThemeConstantOverride("margin_left", 4);
        AddThemeConstantOverride("margin_right", 4);
        AddThemeConstantOverride("margin_bottom", 4);
        AddThemeConstantOverride("margin_top", 4);

        // Add background.
        ColorRect bg = new();
        bg.Color = new Color(0.3f, 0.3f, 0.3f);
        AddChild(bg);

        // Add contents.
        Contents = new();
        AddChild(Contents);

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

    /* Godot overrides. */
    public override void _Ready()
    {
        // Set minimum width.
        CustomMinimumSize = new(GetViewportRect().Size.X * 0.25f, 0);
    }
}
