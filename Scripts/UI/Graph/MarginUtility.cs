using Godot;

namespace Rusty.ActionGraph.Graphs;

/// <summary>
/// An utility class for creating and altering margin containers.
/// </summary>
internal static class MarginUtility
{
    /// <summary>
    /// Set the margins of a margin container.
    /// </summary>
    public static void SetMargins(MarginContainer margin, int marginTop, int marginLeft, int marginRight, int marginBottom)
    {
        margin.AddThemeConstantOverride("margin_top", marginTop);
        margin.AddThemeConstantOverride("margin_left", marginLeft);
        margin.AddThemeConstantOverride("margin_right", marginRight);
        margin.AddThemeConstantOverride("margin_bottom", marginBottom);
    }

    /// <summary>
    /// Create a margin container.
    /// </summary>
    public static MarginContainer Create(int margins) => Create(margins, margins, margins, margins);

    /// <summary>
    /// Create a margin container.
    /// </summary>
    public static MarginContainer Create(int marginTop, int marginLeft, int marginRight, int marginBottom)
    {
        MarginContainer margin = new();
        SetMargins(margin, marginTop, marginLeft, marginRight, marginBottom);
        return margin;
    }
}