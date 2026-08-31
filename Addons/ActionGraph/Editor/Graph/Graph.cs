using Godot;
using Godot.Collections;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An editor graph.
/// </summary>
internal partial class Graph : GraphEdit
{
    /* Public properties. */
    public Array<Node> Nodes { get; } = new();
    public Array<Joint> Joints { get; } = new();
    public Array<Memo> Memos { get; } = new();
    public Array<Frame> Frames { get; } = new();
    public Array<Edge> Edges { get; } = new();

    /* Private properties. */
    private ContextMenu ContextMenu { get; set; }
    private Vector2 SpawnPosition { get; set; }

    /* Constructors. */
    public Graph()
    {
        ContextMenu = new();
        ContextMenu.NodeSelected += (definition) => CreateNode(SpawnPosition, definition);
        ContextMenu.FrameSelected += () => CreateFrame(SpawnPosition);
        ContextMenu.MemoSelected += () => CreateMemo(SpawnPosition);
        AddChild(ContextMenu);
        ContextMenu.Hide();
    }

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

    // Registration.
    /// <summary>
    /// Add a new node type to the editor.
    /// </summary>
    public void RegisterDefinition(NodeDefinition definition) => ContextMenu.AddNode(definition);

    // Element creation.
    /// <summary>
    /// Instantiate a node of some type.
    /// </summary>
    public GraphNode CreateNode(Vector2 position, NodeDefinition definition)
    {
        GraphNode node = new();
        node.Name = "Node" + definition.ID + Joints.Count;
        node.PositionOffset = position;
        Nodes.Add(node);
        AddChild(node);
        return node;
    }

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
        {
            ContextMenu.Position = (Vector2I)GetGlobalMousePosition();
            ContextMenu.Show();
            SpawnPosition = GetMouseCoordinate();
        }
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