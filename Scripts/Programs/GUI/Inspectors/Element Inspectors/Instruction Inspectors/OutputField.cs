namespace Rusty.ISA.Editor;

/// <summary>
/// An output dummy field.
/// </summary>
public partial class OutputField : LabeledElement, IGuiElement
{
    /* Constructors. */
    public OutputField() : base()
    {
        Visible = false;
    }
}