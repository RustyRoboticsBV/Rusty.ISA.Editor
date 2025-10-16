using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A spin-box field.
/// </summary>
public abstract partial class SpinBoxField<T> : GenericField<T, SpinBox>
{
    /* Constructors. */
    public SpinBoxField() : base()
    {
        Field.ValueChanged += OnValueChanged;
    }

    /* Private methods. */
    private void OnValueChanged(double value)
    {
        InvokeChanged();
    }
}