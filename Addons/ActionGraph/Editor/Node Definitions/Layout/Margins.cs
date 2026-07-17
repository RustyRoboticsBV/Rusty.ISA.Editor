using Godot;

namespace Rusty.ActionGraph;

/// <summary>
/// The layout direction of an element.
/// </summary>
[GlobalClass]
public sealed partial class Margins : Resource
{
    /* Public properties. */
    [Export] public int Left { get; set; }
    [Export] public int Right { get; set; }
    [Export] public int Top { get; set; }
    [Export] public int Bottom { get; set; }

    /* Public methods. */
    public void ApplyTo(MarginContainer container)
    {
        container.AddThemeConstantOverride("margin_left", Left);
        container.AddThemeConstantOverride("margin_right", Right);
        container.AddThemeConstantOverride("margin_top", Top);
        container.AddThemeConstantOverride("margin_bottom", Bottom);
    }
}