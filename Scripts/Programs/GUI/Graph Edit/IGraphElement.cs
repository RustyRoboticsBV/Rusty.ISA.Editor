using Godot;
using System;

namespace Rusty.ISA.Editor;

/// <summary>
/// An ISA editor graph element.
/// </summary>
public interface IGraphElement
{
    /* Public properties. */
    public StringName Name { get; set; }
    public string TooltipText { get; set; }

    /// <summary>
    /// The unsnapped position of this element on the graph.
    /// </summary>
    public Vector2 UnsnappedPosition { get; set; }
    /// <summary>
    /// The position of this element on the graph, with grid snapping taken into account.
    /// </summary>
    public Vector2 PositionOffset { get; set; }
    /// <summary>
    /// The current size of this element.
    /// </summary>
    public Vector2 Size { get; set; }
    /// <summary>
    /// Whether or not this element is selected.
    /// </summary>
    public bool Selected { get; set; }

    /// <summary>
    /// The graph edit that this element is contained on.
    /// </summary>
    public GraphEdit GraphEdit { get; }
    /// <summary>
    /// The frame that this element is contained in (if any).
    /// </summary>
    public GraphFrame Frame { get; set; }

    /* Public events. */
    /// <summary>
    /// Invoked if a graph element is clicked with the mouse.
    /// </summary>
    public event Action<IGraphElement> MouseClicked;
    /// <summary>
    /// Invoked if a graph element is released by the mouse.
    /// </summary>
    public event Action<IGraphElement> MouseReleased;
    /// <summary>
    /// Invoked if a graph element is dragged by the mouse.
    /// </summary>
    public event Action<IGraphElement> MouseDragged;

    /* Public methods. */
    /// <summary>
    /// Move the element to a new position.
    /// </summary>
    public void MoveTo(Vector2 position);
    /// <summary>
    /// Check whether or not this element is contained within a specific frame.
    /// </summary>
    public bool IsNestedIn(GraphFrame frame);
}