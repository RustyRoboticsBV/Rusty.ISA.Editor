using Godot;
using Godot.Collections;

namespace Rusty.ActionGraph.Editor;

[GlobalClass]
/// <summary>
/// A graph editor joint, used to split up edges.
/// </summary>
public partial class Graph : GraphEdit
{
    /* Public properties. */
    public Array<Joint> Joints { get; } = new();
    public Array<Memo> Memos { get; } = new();
    public Array<Edge> Edges { get; } = new();

    /* Public methods. */
    /// <summary>
    /// Get the graph coordinate corresponding to the current mouse position.
    /// </summary>
    public Vector2 GetMouseCoordinate() => GetCoordinate(GetGlobalMousePosition());
    /// <summary>
    /// Get the graph coordinate corresponding to the current mouse position.
    /// </summary>
    public Vector2 GetCoordinate(Vector2 globalPosition) => (globalPosition - GlobalPosition + ScrollOffset) / Zoom;

    public Joint AddJoint(Vector2 position)
    {
        Joint joint = new();
        joint.Name = "Joint" + Joints.Count;
        joint.PositionOffset = position;
        Joints.Add(joint);
        AddChild(joint);
        return joint;
    }

    public Memo AddMemo(Vector2 position)
    {
        Memo memo = new();
        memo.Name = "Memo" + Memos.Count;
        memo.PositionOffset = position;
        Memos.Add(memo);
        AddChild(memo);
        return memo;
    }

    public Edge AddEdge(Vector2 start, Vector2 end)
    {
        Edge edge = new();
        edge.Name = "Edge" + Edges.Count;
        edge.PositionOffset = start;
        edge.End = end - start;
        edge.Clicked += OnEdgeClicked;
        Edges.Add(edge);
        AddChild(edge);
        return edge;
    }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        Joint joint = AddJoint(new Vector2(200, 200));

        Memo memo = AddMemo(new Vector2(100, 100));
        memo.Text = "ABCDEFG";

        AddJoint(Vector2.Zero);
        AddEdge(new(100, 100), new(200, 1000));
    }

    /* Private methods. */
    private void OnEdgeClicked(Edge edge, Vector2 position)
    {
        Vector2 start = edge.PositionOffset;
        Vector2 end = edge.PositionOffset + edge.End;

        // Shorten first edge.
        edge.End = position - edge.Position;

        // Add joint.
        Joint joint = AddJoint(position - Joint.RADIUS * Vector2.One);

        // Create second edge.
        AddEdge(position, end);
    }
}