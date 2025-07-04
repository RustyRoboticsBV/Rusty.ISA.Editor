using Godot;
using Godot.Collections;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public partial class GraphEdit : Godot.GraphEdit
{
    /* Public methods. */

    /* Private properties. */
    private List<GraphNode> Nodes { get; } = new();
    private List<GraphComment> Comments { get; } = new();
    private List<GraphFrame> Frames { get; } = new();

    /* Constructors. */
    public GraphEdit()
    {
        DeleteNodesRequest += OnDeleteNodesRequest;
    }

    /* Public methods. */
    public void AddElement(IGraphElement element)
    {
        // Add to typed array.
        switch (element)
        {
            case GraphNode node:
                Nodes.Add(node);
                break;
            case GraphComment comment:
                comment.CustomMinimumSize = new(SnappingDistance * 4 - 10, SnappingDistance * 2 - 10);
                Comments.Add(comment);
                break;
            case GraphFrame frame:
                frame.CustomMinimumSize = new(SnappingDistance * 8, SnappingDistance * 8);
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

    private void OnElementSelected(IGraphElement element) { }

    private void OnElementDeselected(IGraphElement element) { }

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

            Vector2 mousePosition = GetMousePosition();

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
    /// Returns the current mouse position as a graph position offset.
    /// </summary>
    private Vector2 GetMousePosition()
    {
        return (GetGlobalMousePosition() - GlobalPosition + ScrollOffset) / Zoom;
    }
}