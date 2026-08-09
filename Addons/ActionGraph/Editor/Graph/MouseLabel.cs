using Godot;

namespace Rusty.ActionGraph.Editor;

[GlobalClass]
internal partial class MouseLabel : Label
{
    [Export] public Graph Graph { get; set; }

    public override void _Process(double delta)
    {
        if (Graph == null)
            return;

        Vector2 mouse = Graph.GetMouseCoordinate();
        Text = $"X: {Mathf.RoundToInt(mouse.X)}, Y: {Mathf.RoundToInt(mouse.Y)}";
    }
}