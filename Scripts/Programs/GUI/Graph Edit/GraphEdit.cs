using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public partial class GraphEdit : Godot.GraphEdit
{
    /* Public methods. */
    public List<IGraphElement> Elements { get; } = new();
    public HashSet<IGraphElement> Selected { get; } = new();
    public List<GraphNode> Nodes { get; } = new();
    public List<GraphJoint> Joints { get; } = new();
    public List<GraphComment> Comments { get; } = new();
    public List<GraphFrame> Frames { get; } = new();

    public GraphEdges Edges { get; } = new();

    /* Private properties. */
    private bool CanSelectRect => GetChild(GetChildCount(true) - 2, true).GetChild<Line2D>(0, true).Points.Length == 0;
    private bool IsRectSelecting { get; set; }
    private Rect2 SelectRect { get; set; }
    private Rect2 SelectRectVisual { get; set; }

    private bool IsDragging { get; set; }
    private Vector2 LastDragPosition { get; set; }

    /* Public events. */
    public event Action RightClicked;

    /* Constructors. */
    public GraphEdit()
    {
        // Hide unwanted UI elements.
        MinimapEnabled = false;
        ShowArrangeButton = false;

        // Enable disconnection.
        RightDisconnects = true;

        // Subscribe to events.
        DeleteNodesRequest += OnDeleteNodesRequest;
        ConnectionRequest += OnConnectionRequest;
        DisconnectionRequest += OnDisconnectionRequest;
    }

    /* Public methods. */
    /// <summary>
    /// Spawn a node.
    /// </summary>
    public GraphNode SpawnNode(int x, int y)
    {
        GraphNode node = new();
        AddElement(node);
        node.PositionOffset = new(x, y);
        return node;
    }

    /// <summary>
    /// Spawn a joint.
    /// </summary>
    public GraphJoint SpawnJoint(int x, int y)
    {
        GraphJoint joint = new();
        AddElement(joint);
        joint.PositionOffset = new(x, y);
        return joint;
    }

    /// <summary>
    /// Spawn a comment.
    /// </summary>
    public GraphComment SpawnComment(int x, int y)
    {
        GraphComment comment = new();
        AddElement(comment);
        comment.PositionOffset = new(x, y);
        return comment;
    }

    /// <summary>
    /// Spawn a frame.
    /// </summary>
    public GraphFrame SpawnFrame(int x, int y)
    {
        GraphFrame frame = new();
        AddElement(frame);
        frame.PositionOffset = new(x, y);
        return frame;
    }

    /// <summary>
    /// Add an element to the graph.
    /// </summary>
    public void AddElement(IGraphElement element)
    {
        // Add to typed array.
        Elements.Add(element);
        switch (element)
        {
            case GraphNode node:
                Nodes.Add(node);
                break;
            case GraphJoint joint:
                Joints.Add(joint);
                break;
            case GraphComment comment:
                Comments.Add(comment);
                break;
            case GraphFrame frame:
                Frames.Add(frame);
                break;
        }

        // Add as child node.
        AddChild(element as Node);

        // Subscribe to events.
        element.MouseClicked += OnElementClicked;
        element.MouseDragged += OnElementDragged;
        element.MouseReleased += OnElementReleased;
    }

    /// <summary>
    /// Remove an element from the graph.
    /// </summary>
    public void RemoveElement(IGraphElement element)
    {
        // Remove from typed array.
        Elements.Remove(element);
        switch (element)
        {
            case GraphNode node:
                Nodes.Remove(node);
                break;
            case GraphJoint joint:
                Joints.Remove(joint);
                break;
            case GraphComment comment:
                Comments.Remove(comment);
                break;
            case GraphFrame frame:
                Frames.Remove(frame);
                break;
        }

        // Add as child node.
        RemoveChild(element as Node);

        // Remove all tracked edges.
        Edges.RemoveElement(element);
    }

    /// <summary>
    /// Connect two elements on the graph.
    /// </summary>
    public void ConnectElements(IGraphElement fromElement, int fromPort, IGraphElement toElement)
    {
        ConnectNode(fromElement.Name, fromPort, toElement.Name, 0);
        Edges.Connect(fromElement, fromPort, toElement, 0);
    }

    /// <summary>
    /// Select an element.
    /// </summary>
    public void SelectElement(IGraphElement element)
    {
        element.Selected = true;
        if (!Selected.Contains(element))
            Selected.Add(element);
    }

    /// <summary>
    /// Deselect an element.
    /// </summary>
    public void DeselectElement(IGraphElement element)
    {
        element.Selected = false;
        if (Selected.Contains(element))
            Selected.Remove(element);
    }

    /// <summary>
    /// Select all elements.
    /// </summary>
    public void SelectAllElements()
    {
        foreach (IGraphElement element in Elements)
        {
            SelectElement(element);
        }
    }

    /// <summary>
    /// Deselect all elements.
    /// </summary>
    public void DeselectAllElements()
    {
        foreach (IGraphElement element in Elements)
        {
            DeselectElement(element);
        }
    }

    /// <summary>
    /// Convert a global position to a local position offset.
    /// </summary>
    public Vector2 GetPositionOffsetFromGlobalPosition(Vector2 globalPosition)
    {
        return (globalPosition - GlobalPosition + ScrollOffset) / Zoom;
    }

    /// <summary>
    /// Get the current global mouse position and convert it to a position offset.
    /// </summary>
    public Vector2 GetMouseOffset()
    {
        return GetPositionOffsetFromGlobalPosition(GetGlobalMousePosition());
    }

    /* Godot overrides. */
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                IsRectSelecting = true;
                SelectRect = new(GetGlobalMousePosition(), Vector2.Zero);
                SelectRectVisual = new(GetLocalMousePosition(), Vector2.Zero);
                DeselectAllElements();
            }
            else if (!mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                if (IsRectSelecting)
                {
                    IsRectSelecting = false;
                    QueueRedraw();
                }
                if (IsDragging)
                {
                    IsDragging = false;
                }
            }
            else if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Right)
                RightClicked?.Invoke();
        }
        else if (@event is InputEventMouseMotion mouseMotion && IsRectSelecting)
        {
            if (!CanSelectRect)
                IsRectSelecting = false;
            else
            {
                SelectRect = new(SelectRect.Position, GetGlobalMousePosition() - SelectRect.Position);
                SelectRectVisual = new(SelectRectVisual.Position, GetLocalMousePosition() - SelectRectVisual.Position);
                ApplySelectionRect(SelectRect);
            }
            QueueRedraw();
        }
        AcceptEvent();
    }

    public override void _Draw()
    {
        if (IsRectSelecting)
        {
            DrawRect(SelectRectVisual.Abs(), new Color(0.3f, 0.6f, 1f, 0.2f), true);
            DrawRect(SelectRectVisual.Abs(), new Color(0.3f, 0.6f, 1f), false, 2f);
        }
    }

    /* Private methods. */
    private void OnDeleteNodesRequest(Array<StringName> elements)
    {
        foreach (StringName name in elements)
        {
            RemoveElement(GetElement(name));
        }
    }

    private void OnConnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
    {
        // Disconnect from port if it was already connected.
        foreach (var connection in Connections)
        {
            if ((StringName)connection["from_node"] == fromNode && (long)connection["from_port"] == fromPort)
            {
                StringName toNodeOld = (StringName)connection["to_node"];
                int toPortOld = (int)connection["to_port"];
                DisconnectNode(fromNode, (int)fromPort, toNodeOld, toPortOld);
                break;
            }
        }

        // Connect node.
        ConnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
        Edges.Connect(GetElement(fromNode), (int)fromPort, GetElement(toNode), (int)toPort);
    }

    private void OnDisconnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
    {
        DisconnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
        Edges.Disconnect(GetElement(fromNode), (int)fromPort);
    }

    private void OnElementClicked(IGraphElement element)
    {
        IsDragging = true;
        LastDragPosition = GetMouseOffset();
        if (!element.Selected)
        {
            DeselectAllElements();
            SelectElement(element);
        }
    }

    private void OnElementDragged(IGraphElement element)
    {
        Vector2 newDragPosition = GetMouseOffset();
        Vector2 dragDelta = newDragPosition - LastDragPosition;
        foreach (IGraphElement element2 in Elements)
        {
            if (element2.Selected)
                element2.PositionOffset = GridSnap(element2.PositionOffset + dragDelta);
        }
        LastDragPosition = newDragPosition;
    }

    private void OnElementReleased(IGraphElement element)
    {
        IsDragging = false;
    }

    /// <summary>
    /// Retrieve an element on the graph, using its name.
    /// </summary>
    private IGraphElement GetElement(StringName name)
    {
        foreach (Node child in GetChildren())
        {
            if (child.Name == name && child is IGraphElement element)
                return element;
        }
        return null;
    }

    /// <summary>
    /// Snap a position to the grid (if grid snapping has been enabled).
    /// </summary>
    private Vector2 GridSnap(Vector2 position)
    {
        if (SnappingEnabled)
        {
            if (SnappingDistance == 0)
                return position;

            return new Vector2(
                Mathf.Round(position.X / SnappingDistance) * SnappingDistance,
                Mathf.Round(position.Y / SnappingDistance) * SnappingDistance
            );
        }
        return position;
    }

    /// <summary>
    /// Apply selection rect.
    /// </summary>
    private void ApplySelectionRect(Rect2 selectionRect)
    {
        selectionRect = selectionRect.Abs();

        foreach (IGraphElement element in Elements)
        {
            Rect2 elementRect = (element as GraphElement).GetGlobalRect();

            elementRect = new(
                elementRect.Position.X,
                elementRect.Position.Y,
                elementRect.Size.X,
                elementRect.Size.Y
            );

            if (selectionRect.Intersects(elementRect, true))
                SelectElement(element);
            else
                DeselectElement(element);
        }
    }
}