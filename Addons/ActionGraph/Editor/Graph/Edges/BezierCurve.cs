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
    public Vector2 GetControl1()
    {
        float dx = Mathf.Abs(End.X - Start.X);
        float distance = Mathf.Clamp(dx * 0.5f, MinDistance, MaxDistance);

        if (Mathf.IsEqualApprox(Start.Y, End.Y))
            return new Vector2(Start.X + (End.X - Start.X) * 0.33f, Start.Y);

        return new Vector2(Start.X + Mathf.Sign(End.X - Start.X) * distance, Start.Y);
    }

    public Vector2 GetControl2()
    {
        float dx = Mathf.Abs(End.X - Start.X);
        float distance = Mathf.Clamp(dx * 0.5f, MinDistance, MaxDistance);

        if (Mathf.IsEqualApprox(Start.Y, End.Y))
            return new Vector2(Start.X + (End.X - Start.X) * 0.66f, End.Y);

        return new Vector2(End.X - Mathf.Sign(End.X - Start.X) * distance, End.Y);
    }
}