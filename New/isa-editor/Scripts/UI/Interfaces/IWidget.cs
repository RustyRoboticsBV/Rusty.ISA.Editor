using System;

namespace Rusty.ISA;

/// <summary>
/// An ISA widget interface.
/// </summary>
public interface IWidget
{
    /* Public properties. */
    /// <summary>
    /// The title of this widget.
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// The minimum width of the title label.
    /// </summary>
    public int TitleWidth { get; set; }
    /// <summary>
    /// The tooltip text of this widget.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Whether or not the widget is visible.
    /// </summary>
    public bool Visible { get; set; }

    /* Public events. */
    /// <summary>
    /// Event that is invoked when this widget is changed.
    /// </summary>
    public event Action<IWidget> Changed;

    /* Public methods. */
    /// <summary>
    /// Make a deep copy of this widget.
    /// </summary>
    public IWidget Copy();
    /// <summary>
    /// Set the size flags to expand/fill.
    /// </summary>
    public void ExpandFill(bool vertical = true);
}