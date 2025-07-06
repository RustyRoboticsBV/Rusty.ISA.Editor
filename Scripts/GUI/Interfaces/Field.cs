using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An interface for a field; an element that contains a label and a value and can be indented.
/// </summary>
public interface Field
{
    public int LocalIndentation { get; set; }
    public int GlobalIndentation { get; }

    public string LabelText { get; set; }
    public int LabelWidth { get; set; }
    public Color LabelColor { get; set; }
    public int LabelFontSize { get; set; }

    public string TooltipText { get; set; }

    public object Value { get; set; }
}

/// <summary>
/// An interface for a field; an element that contains a label and a value and can be indented.
/// </summary>
public interface Field<T> : Field
{
    public new T Value { get; set; }
}