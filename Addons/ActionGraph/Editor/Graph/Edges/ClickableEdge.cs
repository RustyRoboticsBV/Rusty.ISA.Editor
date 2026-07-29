using Godot;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Editor;

[GlobalClass]
public partial class ClickableEdge : Control
{
    private const float LINE_WIDTH = 3.0f;
    private const float CLICK_DISTANCE = 8.0f;
    private const int SAMPLES = 64;

    private readonly List<BezierCurve> _curves = new();

    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Stop;

        _curves.Add(
            new BezierCurve(
                new Vector2(100, 300),
                new Vector2(700, 100)
            )
        );

        UpdateBounds();
        QueueRedraw();
    }

    private void UpdateBounds()
    {
        if (_curves.Count == 0)
            return;


        Vector2 minPos = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        Vector2 maxPos = new Vector2(float.NegativeInfinity, float.NegativeInfinity);


        foreach (BezierCurve curve in _curves)
        {
            Vector2[] points = SampleCurve(curve, SAMPLES);

            foreach (Vector2 point in points)
            {
                minPos = minPos.Min(point);
                maxPos = maxPos.Max(point);
            }
        }


        Vector2 padding = Vector2.One * (CLICK_DISTANCE + LINE_WIDTH);

        Position = minPos - padding;
        Size = (maxPos - minPos) + padding * 2;
    }

    public override void _Draw()
    {
        foreach (BezierCurve curve in _curves)
        {
            Vector2[] points = SampleCurve(curve, SAMPLES);

            for (int i = 0; i < points.Length; i++)
            {
                points[i] -= Position;
            }

            DrawPolyline(
                points,
                Colors.White,
                LINE_WIDTH
            );
        }
    }

    private Vector2[] SampleCurve(BezierCurve curve, int count)
    {
        Vector2[] points = new Vector2[count + 1];

        Vector2 c1 = curve.GetControl1();
        Vector2 c2 = curve.GetControl2();

        for (int i = 0; i <= count; i++)
        {
            float t = (float)i / count;
            points[i] = CubicBezier(curve.Start, c1,  c2, curve.End, t);
        }

        return points;
    }

    private Vector2 CubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float u = 1.0f - t;

        return
            u * u * u * p0 +
            3.0f * u * u * t * p1 +
            3.0f * u * t * t * p2 +
            t * t * t * p3;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            Vector2 mouse = mouseEvent.Position + Position;

            for (int i = 0; i < _curves.Count; i++)
            {
                float t = FindClickT(_curves[i], mouse);

                if (t >= 0.0f)
                {
                    List<BezierCurve> split = SplitCurve(_curves[i], t);

                    _curves.RemoveAt(i);
                    _curves.Insert(i, split[1]);
                    _curves.Insert(i, split[0]);

                    UpdateBounds();
                    QueueRedraw();

                    AcceptEvent();
                    return;
                }
            }
        }
    }



    private float FindClickT(BezierCurve curve, Vector2 mouse)
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

    private List<BezierCurve> SplitCurve(BezierCurve curve, float t)
    {
        Vector2 p0 = curve.Start;
        Vector2 p1 = curve.GetControl1();
        Vector2 p2 = curve.GetControl2();
        Vector2 p3 = curve.End;

        Vector2 p01 = p0.Lerp(p1, t);
        Vector2 p12 = p1.Lerp(p2, t);
        Vector2 p23 = p2.Lerp(p3, t);

        Vector2 p012 = p01.Lerp(p12, t);
        Vector2 p123 = p12.Lerp(p23, t);

        Vector2 split = p012.Lerp(p123, t);

        return new List<BezierCurve>
        {
            new BezierCurve(p0, split),
            new BezierCurve(split, p3)
        };
    }
}