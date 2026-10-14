using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An input or output port.
/// </summary>1
internal sealed partial class Port : Panel
{
    /* Public properties. */
    /// <summary>
    /// Whether or not this port can be the start point of an edge.
    /// </summary>
    public bool Draggable { get; set; }

    public Color Color
    {
        get => StyleBox.BorderColor;
        set => StyleBox.BorderColor = value;
    }

    /* Private properties. */
    private StyleBoxFlat StyleBox { get; set; }

    /* Constructors. */
    public Port()
    {
        StyleBox = new();
        StyleBox.BgColor = Color.FromHtml("131313");
        StyleBox.BorderColor = Colors.White;
        AddThemeStyleboxOverride("panel", StyleBox);
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        int cornerRadius = (int)(Mathf.Min(Size.X, Size.Y) / 2f);
        StyleBox.CornerRadiusTopLeft = cornerRadius;
        StyleBox.CornerRadiusTopRight = cornerRadius;
        StyleBox.CornerRadiusBottomLeft = cornerRadius;
        StyleBox.CornerRadiusBottomRight = cornerRadius;
        StyleBox.BorderWidthBottom = 2;
        StyleBox.BorderWidthLeft = 2;
        StyleBox.BorderWidthRight = 2;
        StyleBox.BorderWidthTop = 2;
    }
}
