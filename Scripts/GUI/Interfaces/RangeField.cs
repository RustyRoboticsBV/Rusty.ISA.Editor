namespace Rusty.ISA.Editor;

/// <summary>
/// An interface for a range field; an element that contains a label and a value and can be indented.
/// </summary>
public interface RangeField : Field
{
    public object MinValue { get; set; }
    public object MaxValue { get; set; }
}

/// <summary>
/// An interface for a range field; an element that contains a label and a value and can be indented.
/// </summary>
public interface RangeField<T> : Field<T>, RangeField
{
    public new T MinValue { get; set; }
    public new T MaxValue { get; set; }
}