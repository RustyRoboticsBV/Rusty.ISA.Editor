namespace Rusty.ISA;

/// <summary>
/// A widget with a value.
/// </summary>
public interface IValued : IWidget
{
    /* Public properties. */
    /// <summary>
    /// The value of this widget.
    /// </summary>
    public object Value { get; set; }
}

/// <summary>
/// A widget with a value.
/// </summary>
public interface IValued<T> : IValued
{
    /* Public properties. */
    object IValued.Value
    {
        get => Value;
        set => Value = (T)value;
    }

    /// <summary>
    /// The value of this widget.
    /// </summary>
    public new T Value { get; set; }
}