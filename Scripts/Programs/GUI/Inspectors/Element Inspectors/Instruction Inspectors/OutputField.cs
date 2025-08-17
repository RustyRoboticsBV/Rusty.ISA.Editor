namespace Rusty.ISA.Editor;

/// <summary>
/// An output dummy field.
/// </summary>
public partial class OutputField : LabeledElement, IGuiElement, IField<string>
{
    object IField.Value { get => Value; set => Value = value.ToString(); }
    public string Value { get => ""; set { }}

    /* Constructors. */
    public OutputField() : base()
    {
        Visible = false;
    }
}