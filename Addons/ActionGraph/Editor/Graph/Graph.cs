using Godot;
using Godot.Collections;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A graph editor joint, used to split up edges.
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
        ContextMenu.FrameSelected += () => {};
        ContextMenu.MemoSelected += () => CreateMemo(SpawnPosition);
        AddChild(ContextMenu);
        ContextMenu.Hide();
    }

    /* Public methods. */
    /// <summary>
    /// Get the graph coordinate corresponding to the current mouse position.
    /// </summary>
    public Vector2 GetMouseCoordinate() => GetCoordinate(GetGlobalMousePosition());
    /// <summary>
    /// Get the graph coordinate corresponding to the current mouse position.
    /// </summary>
    public Vector2 GetCoordinate(Vector2 globalPosition) => (globalPosition - GlobalPosition + ScrollOffset) / Zoom;

    public void RegisterDefinition(NodeDefinition definition)
    {
        ContextMenu.AddNode(definition);
    }

    public GraphNode CreateNode(Vector2 position, NodeDefinition definition)
    {
        GraphNode node = new(definition);
        node.Name = "Node" + definition.ID + Joints.Count;
        node.PositionOffset = position;
        Nodes.Add(node);
        AddChild(node);
        return node;
    }

    public Joint CreateJoint(Vector2 position)
    {
        Joint joint = new();
        joint.Name = "Joint" + Joints.Count;
        joint.PositionOffset = position;
        Joints.Add(joint);
        AddChild(joint);
        return joint;
    }

    public Memo CreateMemo(Vector2 position)
    {
        Memo memo = new();
        memo.Name = "Memo" + Memos.Count;
        memo.PositionOffset = position;
        Memos.Add(memo);
        AddChild(memo);
        return memo;
    }

    public Edge CreateEdge(Vector2 start, Vector2 end)
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
    private void OnEdgeClicked(Edge edge, Vector2 position)
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

    private void OnContextMenuNodeSelected(NodeDefinition definition)
    {
        
    }

    private void OnContextMenuFrameSelected()
    {
        GD.Print("Frame");
    }

    private void OnContextMenuMemoSelected()
    {
        GD.Print("Memo");
    }
}