using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An interface for a GUI element.
/// </summary>
public interface IGuiElement
{
    /* Public properties. */
    public StringName Name { get; set; }
    public string TooltipText { get; set; }
    public bool Visible { get; set; }

    /* Public events. */
    /// <summary>
    /// Gets invoked when this container changes state.
    /// </summary>
    public event ChangedHandler Changed;

    /* Public methods. */
    /// <summary>
    /// Make a deep copy of this element.
    /// </summary>
    public IGuiElement Copy();
    /// <summary>
    /// Copy the state of this element from another one.
    /// </summary>
    public void CopyFrom(IGuiElement other);
}