using Godot;
using Godot.Collections;
using System;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An editor graph.
/// </summary>
internal sealed partial class Graph : GraphEdit
{
    /* Public properties. */
    public Array<Node> Nodes { get; } = new();
    public Array<Joint> Joints { get; } = new();
    public Array<Memo> Memos { get; } = new();
    public Array<Frame> Frames { get; } = new();
    public Array<Edge> Edges { get; } = new();

    /* Public events. */
    /// <summary>
    /// An event that gets invoked whenever the graph is right-clicked.
    /// </summary>
    public event Action<Vector2> RightClicked;

    /* Public methods. */
    // Coordinates.
    /// <summary>
    /// Get the graph coordinate corresponding to the current mouse position.
    /// </summary>
    public Vector2 GetMouseCoordinate() => GetCoordinate(GetGlobalMousePosition());

    /// <summary>
    /// Get the graph coordinate corresponding to the current mouse position.
    /// </summary>
    public Vector2 GetCoordinate(Vector2 globalPosition) => (globalPosition - GlobalPosition + ScrollOffset) / Zoom;

    // Nodes.
    /// <summary>
    /// Instantiate a node of some type.
    /// </summary>
    public GraphNode CreateNode(Vector2 position, NodeDefinition definition)
    {
        GraphNode node = new();
        node.Name = "Node" + definition.ID + Joints.Count;
        node.PositionOffset = position;
        AddNode(node);
        return node;
    }

    /// <summary>
    /// Add a node to the graph.
    /// </summary>
    public void AddNode(GraphNode node)
    {
        Nodes.Add(node);
        AddChild(node);
    }

    /// <summary>
    /// Remove a node from the graph.
    /// </summary>
    public void RemoveNode(GraphNode node)
    {
        Nodes.Remove(node);
        RemoveChild(node);
    }

    // Joints.
    /// <summary>
    /// Instantiate a joint.
    /// </summary>
    public Joint CreateJoint(Vector2 position)
    {
        Joint joint = new();
        joint.Name = "Joint" + Joints.Count;
        joint.PositionOffset = position;
        Joints.Add(joint);
        AddChild(joint);
        return joint;
    }

    // Memos.
    /// <summary>
    /// Instantiate a memo.
    /// </summary>
    public Memo CreateMemo(Vector2 position)
    {
        Memo memo = new();
        memo.Name = "Memo" + Memos.Count;
        memo.PositionOffset = position;
        Memos.Add(memo);
        AddChild(memo);
        return memo;
    }

    // Frames.
    /// <summary>
    /// Instantiate a frame.
    /// </summary>
    public Frame CreateFrame(Vector2 position)
    {
        Frame frame = new();
        frame.Name = "Frame" + Frames.Count;
        frame.PositionOffset = position;
        Frames.Add(frame);
        AddChild(frame);
        return frame;
    }

    // Edges.
    /// <summary>
    /// Instantiate an edge.
    /// </summary>
    public Edge CreateEdge(Vector2 start, Vector2 end)
    {
        Edge edge = new();
        edge.Name = "Edge" + Edges.Count;
        edge.PositionOffset = start;
        edge.End = end - start;
        edge.Clicked += SplitEdge;
        Edges.Add(edge);
        AddChild(edge);
        return edge;
    }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        Joint joint = CreateJoint(new Vector2(200, 200));

        Memo memo = CreateMemo(new Vector2(100, 100));
        memo.Text = "ABCDEFG";

        CreateJoint(Vector2.Zero);
        CreateEdge(new(100, 100), new(200, 1000));
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Right)
            RightClicked?.Invoke(GetMouseCoordinate());
    }

    /* Private methods. */
    private void SplitEdge(Edge edge, Vector2 position)
    {
        Vector2 start = edge.PositionOffset;
        Vector2 end = edge.PositionOffset + edge.End;

        // Shorten first edge.
        edge.End = position - edge.Position;

        // Add joint.
        Joint joint = CreateJoint(position - Joint.RADIUS * Vector2.One);

        // Create second edge.
        CreateEdge(position, end);
    }
}