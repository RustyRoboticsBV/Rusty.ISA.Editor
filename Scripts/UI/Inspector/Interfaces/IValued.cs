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
    public object Value { get; }

    /* Public methods. */
    /// <summary>
    /// Set the value of this this widget.
    /// </summary>
    public void SetValue(object value);
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
    }

    /// <summary>
    /// The value of this widget.
    /// </summary>
    public new T Value { get; }

    /* Public methods. */
    void IValued.SetValue(object value)
    {
        SetValue((T)value);
    }

    /// <summary>
    /// Set the value of this this widget.
    /// </summary>
    public void SetValue(T value);
}