using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A label that displays the mouse position relative to the graph.
/// </summary>
internal sealed partial class MouseLabel : Label
{
    /* Public properties. */
    [Export] public Graph Graph { get; set; }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        if (Graph == null)
            return;

        Vector2 mouse = Graph.GetMouseCoordinate();
        Text = $"X: {Mathf.RoundToInt(mouse.X)}, Y: {Mathf.RoundToInt(mouse.Y)}";
    }
}