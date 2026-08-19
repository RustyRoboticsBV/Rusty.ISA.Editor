using Godot;
using System;

namespace Rusty.ActionGraph.Editor;

internal interface IWidget
{
    /* Public properties. */
    public Control.MouseFilterEnum MouseFilter { get; set; }
    public Control.SizeFlags SizeFlagsHorizontal { get; set; }
    public Control.SizeFlags SizeFlagsVertical { get; set; }
    public bool Visible { get; set; }
    public string TooltipText { get; set; }

    public UndoRedo UndoRedo { get; set; }

    /* Public methods. */
    /// <summary>
    /// Gets invoked when the widget's state is changed.
    /// </summary>
    public event Action StateChanged;

    /* Public methods. */
    /// <summary>
    /// Duplicate this widget.
    /// </summary>
    public IWidget DuplicateWidget();
}

internal interface IWidget<T> : IWidget
    where T : IWidget
{
    /* Public methods. */
    IWidget IWidget.DuplicateWidget() => DuplicateWidget();

    /// <summary>
    /// Duplicate this widget.
    /// </summary>
    public new T DuplicateWidget();
}