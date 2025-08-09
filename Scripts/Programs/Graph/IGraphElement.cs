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
    /// The position on the graph of this element.
    /// </summary>
    public Vector2 PositionOffset { get; set; }
    /// <summary>
    /// The current size of this element.
    /// </summary>
    public Vector2 Size { get; set; }
    /// <summary>
    /// Whether or not this element is selected.
    /// </summary>
    public bool Selected { get; }

    /// <summary>
    /// The graph edit that this element is contained on.
    /// </summary>
    public GraphEdit GraphEdit { get; }
    /// <summary>
    /// The frame that this element is contained in (if any).
    /// </summary>
    public GraphFrame Frame { get; set; }

    /* Public events. */
    public event Action<IGraphElement> NodeSelected;
    public event Action<IGraphElement> NodeDeselected;
    public event Action<IGraphElement> Dragged;
    public event Action<IGraphElement> DeleteRequest;

    /* Public methods. */
    /// <summary>
    /// Check whether or not this element is contained within a specific frame.
    /// </summary>
    public bool IsNestedIn(GraphFrame frame);

    /// <summary>
    /// Delete this element from its parent graph edit.
    /// </summary>
    public void RequestDelete();
}