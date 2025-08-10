using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A line-edit field.
/// </summary>
public abstract partial class LineEditField<T> : GenericField<T, LineEdit>
{
    /* Constructors. */
    public LineEditField() : base()
    {
        Field.TextChanged += OnTextChanged;
    }

    /* Private methods. */
    private void OnTextChanged(string str)
    {
        InvokeChanged();
    }
}