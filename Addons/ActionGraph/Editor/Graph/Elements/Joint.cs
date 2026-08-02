using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A graph editor joint, used to split up edges.
/// </summary>
public partial class Joint : GraphElement
{
    /* Public constants. */
    public static float RADIUS = 5f;

    /* Public properties. */
    [Export] public Color FillColor { get; set; } = Colors.White;
    [Export] public Color OutlineColor { get; set; } = Colors.Gray;

    [Export] public Color SelectedFillColor { get; set; } = Colors.Gray;
    [Export] public Color SelectedOutlineColor { get; set; } = Colors.DodgerBlue;

    /* Private properties. */
    private bool Hovered { get; set; }

    /* Public methods. */
    public Vector2 GetGlobalCenter() => GlobalPosition + Size / 2f;

    /* Godot overrides. */
    public override void _Ready()
    {
        OffsetBottom = -RADIUS;
        OffsetLeft = -RADIUS;
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
