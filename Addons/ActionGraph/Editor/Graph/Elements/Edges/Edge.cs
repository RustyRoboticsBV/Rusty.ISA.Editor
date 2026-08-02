using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A graph editor edge.
/// </summary>
public partial class Edge : GraphElement
{
    /* Private constants. */
    private const float LINE_WIDTH = 3.0f;
    private const float CLICK_DISTANCE = 8.0f;
    private const int SAMPLES = 64;

    /* Fields. */
    private Vector2 end;

    /* Public properties. */
    [Export] public Vector2 End
    {
        get => end;
        set
        {
            end = value;
            UpdateCurve();
        }
    }

    /* Private properties. */
    private BezierCurve Curve { get; set; }

    /* Public events. */
    public event Action<Edge, Vector2> Clicked;

    /* Godot overrides. */
    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Pass;
        Draggable = false;
        Selectable = false;

        UpdateCurve();
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            Vector2 mouse = mouseEvent.Position + PositionOffset;

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

    public override void _Draw()
    {
        // Update curve.
        Curve.Start = PositionOffset;
        Curve.End = End;

        // Sample curve.
        Vector2[] points = Curve.Sample(SAMPLES);
        for (int i = 0; i < points.Length; i++)
        {
            points[i] -= PositionOffset;
        }

        // Draw curve.
        DrawPolyline(points, Colors.White, LINE_WIDTH);
    }

    /* Private methods. */
    private void UpdateCurve()
    {
        // Update curve.
        if (Curve == null)
            Curve = new(Vector2.Zero, End);
        else
            Curve.End = End;

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
        PositionOffset = minPos - padding;
        Size = maxPos - minPos + padding * 2;

        // Update graphic.
        QueueRedraw();
    }
}