using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A bezier curve.
/// </summary>
internal class BezierCurve
{
    /* Public properties. */
    public Vector2 Start { get; set; }
    public Vector2 End { get; set; }

    /* Constants. */
    private const float MinDistance = 40.0f;
    private const float MaxDistance = 200.0f;

    /* Constructors. */
    public BezierCurve(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
    }

    /* Public methods. */
    /// <summary>
    /// Get the first control point.
    /// </summary>
    public Vector2 GetControl1()
    {
        float dx = Mathf.Abs(End.X - Start.X);
        float distance = Mathf.Clamp(dx * 0.5f, MinDistance, MaxDistance);

        if (Mathf.IsEqualApprox(Start.Y, End.Y))
            return new Vector2(Start.X + (End.X - Start.X) * 0.33f, Start.Y);

        return new Vector2(Start.X + Mathf.Sign(End.X - Start.X) * distance, Start.Y);
    }

    /// <summary>
    /// Get the second control point.
    /// </summary>
    public Vector2 GetControl2()
    {
        float dx = Mathf.Abs(End.X - Start.X);
        float distance = Mathf.Clamp(dx * 0.5f, MinDistance, MaxDistance);

        if (Mathf.IsEqualApprox(Start.Y, End.Y))
            return new Vector2(Start.X + (End.X - Start.X) * 0.66f, End.Y);

        return new Vector2(End.X - Mathf.Sign(End.X - Start.X) * distance, End.Y);
    }

    /// <summary>
    /// Convert the curve to a list of points.
    /// </summary>
    public Vector2[] Sample(int count)
    {
        // Get control points.
        Vector2 c1 = GetControl1();
        Vector2 c2 = GetControl2();

        // Get points.
        Vector2[] points = new Vector2[count + 1];
        for (int i = 0; i <= count; i++)
        {
            float t = (float)i / count;
            points[i] = CubicBezier(Start, c1, c2, End, t);
        }
        return points;
    }

    /// <summary>
    /// Get a point on the curve.
    /// </summary>
    public Vector2 GetPoint(float t)
    {
        t = Mathf.Clamp(t, 0f, 1f);
        float u = 1.0f - t;

        return
            u * u * u * Start +
            3.0f * u * u * t * GetControl1() +
            3.0f * u * t * t * GetControl2() +
            t * t * t * End;
    }

    /* Private methods. */
    private static Vector2 CubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float u = 1.0f - t;

        return
            u * u * u * p0 +
            3.0f * u * u * t * p1 +
            3.0f * u * t * t * p2 +
            t * t * t * p3;
    }
}