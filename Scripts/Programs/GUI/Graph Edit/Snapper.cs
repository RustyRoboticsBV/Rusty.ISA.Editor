using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// Utility class for grid snapping graph elements.
/// </summary>
public static class Snapper
{
    /// <summary>
    /// Snap a graph elemnt to its parent graph edit's grid (if snapping has been enabled).
    /// </summary>
    public static void SnapToGrid(IGraphElement element)
    {
        if (element.GraphEdit != null && element.GraphEdit.SnappingEnabled)
        {
            float snappingDistance = element.GraphEdit.SnappingDistance;

            if (snappingDistance != 0)
            {
                element.PositionOffset = Snap(element.UnsnappedPosition, snappingDistance);

                if (element is GraphFrame frame)
                    frame.Size = Snap(frame.UnsnappedSize, snappingDistance);
            }
        }
    }

    /* Private methods. */
    private static Vector2 Snap(Vector2 value, float snappingDistance)
    {
        return new Vector2(
            Mathf.Round(value.X / snappingDistance) * snappingDistance,
            Mathf.Round(value.Y / snappingDistance) * snappingDistance
        );
    }
}