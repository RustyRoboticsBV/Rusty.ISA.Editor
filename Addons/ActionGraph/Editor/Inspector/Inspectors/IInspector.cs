namespace Rusty.ActionGraph.Editor;

/// <summary>
/// An interface for inspectors.
/// </summary>
internal interface IInspector : IWidget
{
    /* Public properties. */
    IWidget IWidget.DuplicateWidget() => DuplicateWidget();
    /// <summary>
    /// Duplicate this inspector.
    /// </summary>
    public new IInspector DuplicateWidget();
}

/// <summary>
/// An interface for inspectors.
/// </summary>
internal interface IInspector<T> : IInspector, IWidget<T>
    where T : IInspector
{
    /* Public methods. */
    IInspector IInspector.DuplicateWidget() => DuplicateWidget();
    IWidget IWidget.DuplicateWidget() => DuplicateWidget();
    T IWidget<T>.DuplicateWidget() => DuplicateWidget();

    /// <summary>
    /// Duplicate this inspector.
    /// </summary>
    public new T DuplicateWidget();
}