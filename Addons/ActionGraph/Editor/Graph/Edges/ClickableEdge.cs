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
        QueueRedraw();
    }

    /*public override void _Draw()
    {
        // Sample curve.
        UpdateCurve();
        Vector2[] points = Curve.Sample(SAMPLES);
        for (int i = 0; i < points.Length; i++)
        {
            points[i] -= Position;
        }

        // Draw curve.
        DrawPolyline(points, Colors.White, LINE_WIDTH);
    }*/

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            Vector2 mouse = mouseEvent.Position + Position;

            UpdateCurve();
            float t = FindClickT(Curve, mouse);
            if (t >= 0.0f)
            {
                Vector2 pos = Curve.GetPoint(t);
                Clicked?.Invoke(this, pos);
                End = pos;

                QueueRedraw();

                AcceptEvent();
                return;
            }
        }
    }

    /* Private methods. */
    private void UpdateCurve()
    {
        if (Curve == null)
            Curve = new(Start, End);
        else
        {
            Curve.Start = Start;
            Curve.End = End;
        }

        UpdateBounds();

        Vector2[] points = Curve.Sample(SAMPLES);
        for (int i = 0; i < points.Length; i++)
        {
            points[i] -= Position;
        }
        Line.Points = points;
    }

    private void UpdateBounds()
    {
        // Find min/max position.
        Vector2 minPos = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        Vector2 maxPos = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

        Vector2[] points = Curve.Sample(SAMPLES);
        foreach (Vector2 point in points)
        {
            minPos = minPos.Min(point);
            maxPos = maxPos.Max(point);
        }

        // Update size and position.
        Vector2 padding = Vector2.One * (CLICK_DISTANCE + LINE_WIDTH);
        Position = minPos - padding;
        Size = maxPos - minPos + padding * 2;
    }

    private static float FindClickT(BezierCurve curve, Vector2 mouse)
    {
        Vector2[] points = curve.Sample(SAMPLES);

        float bestDistance = float.PositiveInfinity;
        float bestT = -1.0f;

        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector2 closest = Geometry2D.GetClosestPointToSegment(mouse, points[i], points[i + 1]);

            float distance = mouse.DistanceTo(closest);
            if (distance < bestDistance)
            {
                bestDistance = distance;

                float segmentLength = points[i].DistanceTo(points[i + 1]);

                float localT = 0.0f;
                if (segmentLength > 0)
                    localT = points[i].DistanceTo(closest) / segmentLength;

                bestT = Mathf.Lerp((float)i / SAMPLES, (float)(i + 1) / SAMPLES, localT);
            }
        }

        return bestDistance <= CLICK_DISTANCE ? bestT : -1.0f;
    }
}