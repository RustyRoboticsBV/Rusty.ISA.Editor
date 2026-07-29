using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

[GlobalClass]
public partial class ClickableEdge : Control
{
    /* Private constants. */
    private const float LINE_WIDTH = 3.0f;
    private const float CLICK_DISTANCE = 8.0f;
    private const int SAMPLES = 64;

    /* Public properties. */
    [Export] Vector2 Start { get; set; } = new(100, 300);
    [Export] Vector2 End { get; set; } = new(700, 100);

    /* Private properties. */
    private BezierCurve Curve { get; set; }
    private Line2D Line { get; set; }

    /* Public events. */
    public event Action<ClickableEdge, Vector2> Clicked;

    /* Godot overrides. */
    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Stop;

        Line = new();
        Line.Width = LINE_WIDTH;
        AddChild(Line);

        UpdateCurve();
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            Vector2 mouse = mouseEvent.Position + Position;

            float t = Curve.FindPosition(SAMPLES, mouse, CLICK_DISTANCE);
            if (t >= 0.0f)
            {
                Vector2 pos = Curve.GetPoint(t);
                Clicked?.Invoke(this, pos);
                End = pos;

                UpdateCurve();
                AcceptEvent();
            }
        }
    }

    /* Private methods. */
    private void UpdateCurve()
    {
        // Update curve.
        if (Curve == null)
            Curve = new(Start, End);
        else
        {
            Curve.Start = Start;
            Curve.End = End;
        }

        // Update bounds.
        Vector2 minPos = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        Vector2 maxPos = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

        Vector2[] points = Curve.Sample(SAMPLES);
        foreach (Vector2 point in points)
        {
            minPos = minPos.Min(point);
            maxPos = maxPos.Max(point);
        }

        Vector2 padding = Vector2.One * (CLICK_DISTANCE + LINE_WIDTH);
        Position = minPos - padding;
        Size = maxPos - minPos + padding * 2;

        // Update graphic.
        Vector2[] localPoints = Curve.Sample(SAMPLES);
        for (int i = 0; i < localPoints.Length; i++)
        {
            localPoints[i] -= Position;
        }
        Line.Points = localPoints;
    }
}