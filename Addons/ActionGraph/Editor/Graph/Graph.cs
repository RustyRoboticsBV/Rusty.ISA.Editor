using Godot;
using Godot.Collections;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A graph editor joint, used to split up edges.
/// </summary>
internal partial class Graph : GraphEdit
{
    /* Public properties. */
    public Array<Joint> Joints { get; } = new();
    public Array<Memo> Memos { get; } = new();
    public Array<Edge> Edges { get; } = new();

    /* Private properties. */
    private ContextMenu ContextMenu { get; set; }

    /* Constructors. */
    public Graph()
    {
        ContextMenu = new();
        ContextMenu.NodeSelected += OnContextMenuNodeSelected;
        ContextMenu.FrameSelected += OnContextMenuFrameSelected;
        ContextMenu.MemoSelected += OnContextMenuMemoSelected;
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

    private void OnContextMenuNodeSelected(NodeDefinition index)
    {
        GD.Print("Node " + index);
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