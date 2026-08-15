namespace Rusty.ActionGraph.Editor;

internal interface IField : IWidget
{
    /* Public properties. */
    public string TitleText { get; set; }
    public int TitleWidth { get; set; }
    
    /* Public methods. */
    IWidget IWidget.DuplicateWidget() => DuplicateWidget();
    /// <summary>
    /// Duplicate this field.
    /// </summary>
    public new IField DuplicateWidget();
}

internal interface IField<T> : IField, IWidget<T>
    where T : IField
{
    /* Public methods. */
    IField IField.DuplicateWidget() => DuplicateWidget();
    IWidget IWidget.DuplicateWidget() => DuplicateWidget();
    T IWidget<T>.DuplicateWidget() => DuplicateWidget();

    /// <summary>
    /// Duplicate this field.
    /// </summary>
    public new T DuplicateWidget();
}