using System.Collections.Generic;

namespace Rusty.ActionGraph;

/// <summary>
/// A widget group.
/// </summary>
public interface IGroup : IWidget
{
    /* Public properties. */
    /// <summary>
    /// The children of this widget.
    /// </summary>
    public List<IWidget> Children { get; }
    /// <summary>
    /// Get the number of child widgets.
    /// </summary>
    public int WidgetCount => Children.Count;

    /* Public methods. */
    /// <summary>
    /// Add a widget to this group.
    /// </summary>
    public void AddWidget(IWidget widget);

    /// <summary>
    /// Get the child widget at some index.
    /// </summary>
    public IWidget GetWidgetAt(int index) => Children[index];
}