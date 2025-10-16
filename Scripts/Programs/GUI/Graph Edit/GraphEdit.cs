using Godot;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

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

    private bool ShiftHeld { get; set; }

    private HashSet<IGraphElement> RectSelected { get; set; } = new();

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
        node.UnsnappedPosition = new(x, y);
        return node;
    }

    /// <summary>
    /// Spawn a joint.
    /// </summary>
    public GraphJoint SpawnJoint(int x, int y)
    {
        GraphJoint joint = new();
        AddElement(joint);
        joint.UnsnappedPosition = new(x, y);
        return joint;
    }

    /// <summary>
    /// Spawn a comment.
    /// </summary>
    public GraphComment SpawnComment(int x, int y)
    {
        GraphComment comment = new();
        AddElement(comment);
        comment.UnsnappedPosition = new(x, y);
        return comment;
    }

    /// <summary>
    /// Spawn a frame.
    /// </summary>
    public GraphFrame SpawnFrame(int x, int y)
    {
        GraphFrame frame = new();
        AddElement(frame);
        frame.UnsnappedPosition = new(x, y);
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

        // Remove from selection set.
        if (Selected.Contains(element))
        {
            Selected.Remove(element);
            element.Selected = false;
        }
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
        // Mouse buttons.
        if (@event is InputEventMouseButton mouseButton)
        {
            // Left mouse pressed.
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                // Start rect selecting.
                IsRectSelecting = true;
                SelectRect = new(GetGlobalMousePosition(), Vector2.Zero);
                SelectRectVisual = new(GetLocalMousePosition(), Vector2.Zero);

                // Deselect if shift is not held.
                if (!ShiftHeld)
                    DeselectAllElements();
            }

            // Left mouse released.
            else if (!mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                // Stop rect selection.
                if (IsRectSelecting)
                {
                    IsRectSelecting = false;
                    foreach (IGraphElement element in RectSelected)
                    {
                        SelectElement(element);
                    }
                    RectSelected.Clear();
                    QueueRedraw();
                }

                // Stop dragging.
                if (IsDragging)
                    EndDragging();
            }

            // Right mouse pressed.
            else if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Right)
                RightClicked?.Invoke();
        }

        // Mouse motion.
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

        // Keyboard key pressed.
        else if (@event is InputEventKey key)
        {
            // Delete.
            if (key.Keycode == Key.Delete && key.Pressed)
            {
                foreach (IGraphElement selected in Selected)
                {
                    RemoveElement(selected);
                }
            }

            // Shift.
            else if (key.Keycode == Key.Shift)
                ShiftHeld = key.Pressed;
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
            if (!ShiftHeld)
                DeselectAllElements();
            SelectElement(element);
        }
    }

    private void OnElementDragged(IGraphElement element)
    {
        Vector2 newDragPosition = GetMouseOffset();
        Vector2 dragDelta = newDragPosition - LastDragPosition;
        foreach (IGraphElement selected in Selected)
        {
            selected.MoveTo(selected.UnsnappedPosition + dragDelta);
        }
        LastDragPosition = newDragPosition;
    }

    private void OnElementReleased(IGraphElement element)
    {
        EndDragging();
    }


    private void EndDragging()
    {
        IsDragging = false;

        // Snap to grid.
        foreach (IGraphElement selected in Selected)
        {
            Snapper.SnapToGrid(selected);
            selected.UnsnappedPosition = selected.PositionOffset;
        }

        // Check if we must add the elements to a frame.
        GraphFrame targetFrame = null;
        foreach (GraphFrame frame in Frames)
        {
            if (frame.Frame == null)
            {
                targetFrame = MustNestIn(frame, GetGlobalMousePosition());
                if (targetFrame != null)
                    break;
            }
        }

        // Add the elements to the new frame.
        if (targetFrame != null)
        {
            foreach (IGraphElement selected in Selected)
            {
                if (selected.Frame != null)
                    selected.Frame.RemoveElement(selected);
                targetFrame.AddElement(selected);
            }
            targetFrame.FitAroundElements();
        }

        // Remove elements from frames.
        else
        {
            HashSet<GraphFrame> changedFrames = new();
            foreach (IGraphElement selected in Selected)
            {
                if (selected.Frame != null)
                {
                    changedFrames.Add(selected.Frame);
                    selected.Frame.RemoveElement(selected);
                }
            }
            foreach (GraphFrame changedFrame in changedFrames)
            {
                changedFrame.FitAroundElements();
            }
        }
    }

    /// <summary>
    /// Check if a global point will cause a nesting event in some frame if an element is dragged to it. It returns null if no
    /// nesting will happen, and a reference to a frame if it must nest in the frame or one of it child frames.
    /// </summary>
    private GraphFrame MustNestIn(GraphFrame frame, Vector2 globalPosition)
    {
        // We cannot nest in selected frames or frames with selected parent frames.
        if (Selected.Contains(frame))
            return null;

        // Check if we must nest in a child frame.
        foreach (IGraphElement element in frame.Elements)
        {
            if (element is GraphFrame child)
            {
                GraphFrame result = MustNestIn(child, globalPosition);
                if (result != null)
                    return result;
            }
        }

        // Check if we must nest in this frame.
        if (!Selected.Contains(frame) && frame.GetGlobalRect().HasPoint(globalPosition))
            return frame;

        // No nesting needed.
        return null;
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
    /// Apply selection rect.
    /// </summary>
    private void ApplySelectionRect(Rect2 selectionRect)
    {
        selectionRect = selectionRect.Abs();
        RectSelected.Clear();
        foreach (IGraphElement element in Elements)
        {
            Rect2 elementRect = (element as GraphElement).GetGlobalRect();

            elementRect = new(
                elementRect.Position.X,
                elementRect.Position.Y,
                elementRect.Size.X,
                elementRect.Size.Y
            );

            if (!Selected.Contains(element))
            {
                if (selectionRect.Intersects(elementRect, true))
                {
                    RectSelected.Add(element);
                    element.Selected = true;
                }
                else
                    element.Selected = false;
            }
        }
    }

    /// <summary>
    /// Align an element's unsnapped position to the grid.
    /// </summary>
    private void AlignToGrid(IGraphElement element)
    {
        Snapper.SnapToGrid(element);
        element.UnsnappedPosition = element.PositionOffset;
        element.Frame?.FitAroundElements();
    }
}