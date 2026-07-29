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

    /* Public events. */
    public event Action<ClickableEdge, Vector2> Clicked;

    /* Godot overrides. */
    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Stop;

        UpdateBounds();
        QueueRedraw();
    }

    public override void _Draw()
    {
        // Update curve.
        Curve.Start = Start;
        Curve.End = End;

        // Sample curve.
        Vector2[] points = SampleCurve(Curve, SAMPLES);
        for (int i = 0; i < points.Length; i++)
        {
            points[i] -= Position;
        }

        // Draw curve.
        DrawPolyline(points, Colors.White, LINE_WIDTH);
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            Vector2 mouse = mouseEvent.Position + Position;

            UpdateCurve();
            float t = FindClickT(Curve, mouse);
            if (t >= 0.0f)
            {
                Clicked?.Invoke(this, mouse);
                End = mouse;

                UpdateBounds();
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
    }

    private void UpdateBounds()
    {
        // Find min/max position.
        Vector2 minPos = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        Vector2 maxPos = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

        UpdateCurve();
        Vector2[] points = SampleCurve(Curve, SAMPLES);
        foreach (Vector2 point in points)
        {
            minPos = minPos.Min(point);
            maxPos = maxPos.Max(point);
        }

        Vector2 padding = Vector2.One * (CLICK_DISTANCE + LINE_WIDTH);

        // Update size and position.
        Position = minPos - padding;
        Size = maxPos - minPos + padding * 2;
    }

    private static Vector2[] SampleCurve(BezierCurve curve, int count)
    {
        // Get control points.
        Vector2 c1 = curve.GetControl1();
        Vector2 c2 = curve.GetControl2();

        // Get points.
        Vector2[] points = new Vector2[count + 1];
        for (int i = 0; i <= count; i++)
        {
            float t = (float)i / count;
            points[i] = CubicBezier(curve.Start, c1, c2, curve.End, t);
        }
        return points;
    }

    private static Vector2 CubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float u = 1.0f - t;

        return
            u * u * u * p0 +
            3.0f * u * u * t * p1 +
            3.0f * u * t * t * p2 +
            t * t * t * p3;
    }

    private static float FindClickT(BezierCurve curve, Vector2 mouse)
    {
        Vector2[] points = SampleCurve(curve, SAMPLES);

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