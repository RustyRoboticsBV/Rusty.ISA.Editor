using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public partial class GraphEdit : Godot.GraphEdit
{
    /* Public methods. */
    public List<GraphNode> Nodes { get; } = new();
    public List<GraphJoint> Joints { get; } = new();
    public List<GraphComment> Comments { get; } = new();
    public List<GraphFrame> Frames { get; } = new();

    public GraphEdges Edges { get; } = new();

    /* Public events. */
    public event Action<IGraphElement> AddedElement;
    public event Action<IGraphElement> SelectedElement;
    public event Action<IGraphElement> DeselectedElement;
    public event Action<IGraphElement> DeletedElement;
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
    public GraphNode SpawnNode(int x, int y)
    {
        GraphNode node = new();
        AddElement(node);
        node.PositionOffset = new(x, y);
        return node;
    }

    public GraphJoint SpawnJoint(int x, int y)
    {
        GraphJoint joint = new();
        AddElement(joint);
        joint.PositionOffset = new(x, y);
        return joint;
    }

    public GraphComment SpawnComment(int x, int y)
    {
        GraphComment comment = new();
        AddElement(comment);
        comment.PositionOffset = new(x, y);
        return comment;
    }

    public GraphFrame SpawnFrame(int x, int y)
    {
        GraphFrame frame = new();
        AddElement(frame);
        frame.PositionOffset = new(x, y);
        return frame;
    }

    public void AddElement(IGraphElement element)
    {
        // Add to typed array.
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
        element.NodeSelected += OnElementSelected;
        element.NodeDeselected += OnElementDeselected;
        element.Dragged += OnElementDragged;
        element.DeleteRequest += OnElementDeleteRequest;

        // Invoke event.
        AddedElement?.Invoke(element);
    }

    public void RemoveElement(IGraphElement element)
    {
        // Remove from typed array.
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

        // Subscribe to events.
        element.NodeSelected -= OnElementSelected;
        element.NodeDeselected -= OnElementDeselected;
        element.Dragged -= OnElementDragged;
        element.DeleteRequest -= OnElementDeleteRequest;
    }

    /// <summary>
    /// Convert a global position to a local position offset.
    /// </summary>
    public Vector2 GetPositionOffsetFromGlobalPosition(Vector2 globalPosition)
    {
        return (globalPosition - GlobalPosition + ScrollOffset) / Zoom;
    }

    /* Godot overrides. */
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Right)
            RightClicked?.Invoke();
    }

    /* Private methods. */
    private void OnDeleteNodesRequest(Array<StringName> elements)
    {
        foreach (StringName name in elements)
        {
            IGraphElement element = GetElement(name);
            element?.RequestDelete();
        }
    }

    private void OnElementSelected(IGraphElement element)
    {
        SelectedElement?.Invoke(element);
    }

    private void OnElementDeselected(IGraphElement element)
    {
        DeselectedElement?.Invoke(element);
    }

    private void OnElementDragged(IGraphElement element)
    {
        GraphFrame frame = null;

        // Check if we've just dragged the element into a frame.
        for (int i = 0; i < Frames.Count; i++)
        {
            if (element == Frames[i])
                continue;

            float x1 = Frames[i].PositionOffset.X;
            float y1 = Frames[i].PositionOffset.Y;
            float x2 = x1 + Frames[i].Size.X;
            float y2 = y1 + Frames[i].Size.Y;

            Vector2 globalMousePosition = GetGlobalMousePosition();
            Vector2 mousePosition = GetPositionOffsetFromGlobalPosition(globalMousePosition);

            if (mousePosition.X > x1 && mousePosition.X < x2 && mousePosition.Y > y1 && mousePosition.Y < y2
                && (frame == null || Frames[i].IsNestedIn(frame)))
            {
                frame = Frames[i];
            }
        }

        // If the new frame is different from the old one...
        if (element.Frame != frame)
        {
            // Remove from old frame.
            if (element.Frame != null)
                element.Frame.RemoveElement(element);

            // Add to new frame.
            if (frame != null)
            {
                for (int i = 0; i < GetChildCount(); i++)
                {
                    if (GetChild(i) is IGraphElement selected && selected.Selected)
                    {
                        frame.AddElement(selected);
                    }
                }
            }
        }
    }

    private void OnElementDeleteRequest(IGraphElement element)
    {
        // Remove element from graph.
        RemoveChild(element as Node);
        Edges.RemoveElement(element);

        // Remove it from the typed element array.
        switch (element)
        {
            case GraphNode node:
                Nodes.Remove(node);
                break;
            case GraphComment comment:
                Comments.Remove(comment);
                break;
            case GraphFrame frame:
                Frames.Remove(frame);
                break;
        }

        // Invoke deleted event.
        DeletedElement?.Invoke(element);
    }

    private void OnConnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
    {
        // Disconnect from port if it was already connected.
        foreach (var connection in Connections)
        {
            if ((StringName)connection["from_node"] == fromNode
                && (long)connection["from_port"] == fromPort)
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
}