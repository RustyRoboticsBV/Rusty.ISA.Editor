using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A graph editor joint, used to split up edges.
/// </summary>
public partial class Joint : GraphElement
{
    public static float RADIUS = 10f;

    [Export] public Color FillColor = Colors.White;
    [Export] public Color OutlineColor = Colors.Gray;

    [Export] public Color SelectedFillColor = Colors.Gray;
    [Export] public Color SelectedOutlineColor = Colors.DodgerBlue;

    private bool Hovered { get; set; }

    public override void _Ready()
    {
        CustomMinimumSize = Vector2.One * RADIUS * 2;
        Size = CustomMinimumSize;

        Draggable = true;
        Selectable = true;

        MouseEntered += () =>
        {
            Hovered = true;
            QueueRedraw();
        };

        MouseExited += () =>
        {
            Hovered = false;
            QueueRedraw();
        };

        QueueRedraw();
    }

    public override void _Draw()
    {
        Vector2 center = Size / 2f;

        Color fill = Hovered || Selected ? SelectedFillColor : FillColor;
        Color outline = Hovered || Selected ? SelectedOutlineColor : OutlineColor;

        DrawCircle(center, RADIUS, fill);
        DrawArc(center, RADIUS, 0, Mathf.Tau, 64, outline, 2.0f);
    }
}
