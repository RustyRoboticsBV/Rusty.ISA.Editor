using Godot;

namespace Rusty.ActionGraph.Graphs;

/// <summary>
/// An utility class for creating and altering panels.
/// </summary>
internal static class PanelUtility
{
    /// <summary>
    /// Set the style of a panel container.
    /// </summary>
    public static void SetPanelStyle(PanelContainer panel, StyleBox style)
    {
        panel.AddThemeStyleboxOverride("panel", style);
    }

    /// <summary>
    /// Create a flat style box with no border.
    /// </summary>
    public static StyleBoxFlat GetStyleBox(Color bgColor,
        int cornerRadiusTopLeft, int cornerRadiusTopRight, int cornerRadiusBottomLeft, int cornerRadiusBottomRight)
    => GetStyleBox(bgColor, cornerRadiusTopLeft, cornerRadiusTopRight, cornerRadiusBottomLeft, cornerRadiusBottomRight,
        Colors.Transparent, 0, 0, 0, 0);

    /// <summary>
    /// Create a flat style box with a border.
    /// </summary>
    public static StyleBoxFlat GetStyleBox(Color bgColor,
        int cornerRadiusTopLeft, int cornerRadiusTopRight, int cornerRadiusBottomLeft, int cornerRadiusBottomRight,
        Color borderColor, int borderWidthTop, int borderWidthLeft, int borderWidthRight, int borderWidthBottom)
    {
        StyleBoxFlat styleBox = new();
        styleBox.BgColor = bgColor;
        styleBox.CornerRadiusTopLeft = cornerRadiusTopLeft;
        styleBox.CornerRadiusTopRight = cornerRadiusTopRight;
        styleBox.CornerRadiusBottomLeft = cornerRadiusBottomLeft;
        styleBox.CornerRadiusBottomRight = cornerRadiusBottomRight;
        styleBox.BorderColor = borderColor;
        styleBox.BorderWidthTop = borderWidthTop;
        styleBox.BorderWidthLeft = borderWidthLeft;
        styleBox.BorderWidthRight = borderWidthRight;
        styleBox.BorderWidthBottom = borderWidthBottom;
        return styleBox;
    }
}