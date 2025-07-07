namespace Rusty.ISA.Editor;

/// <summary>
/// An interface for a range field; an element that contains a label and a value and can be indented.
/// </summary>
public interface IRangeField : IField
{
    public object MinValue { get; set; }
    public object MaxValue { get; set; }
}

/// <summary>
/// An interface for a range field; an element that contains a label and a value and can be indented.
/// </summary>
public interface IRangeField<T> : IField<T>, IRangeField
{
    public new T MinValue { get; set; }
    public new T MaxValue { get; set; }
}