using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A label that displays the mouse position relative to the graph.
/// </summary>
internal sealed partial class MouseLabel : HBoxContainer
{
    /* Public properties. */
    [Export] public Graph Graph { get; set; }

    /* Private properties. */
    private Label X { get; set; }
    private Label Comma { get; set; }
    private Label Y { get; set; }

    /* Constructors. */
    public MouseLabel()
    {
        AddThemeConstantOverride("separation", 0);

        X = new();
        X.Name = "X";
        X.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(X);

        Comma = new();
        X.Name = "Comma";
        Comma.Text = ", ";
        Comma.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
        AddChild(Comma);

        Y = new();
        Y.Name = "Y";
        Y.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        AddChild(Y);
    }

    /* Godot overrides. */
    public override void _Process(double delta)
    {
        if (Graph == null)
            return;

        Vector2 mouse = Graph.GetMouseCoordinate();
        X.Text = $"X: {Mathf.RoundToInt(mouse.X)}";
        Y.Text = $"Y: {Mathf.RoundToInt(mouse.Y)}";
    }
}