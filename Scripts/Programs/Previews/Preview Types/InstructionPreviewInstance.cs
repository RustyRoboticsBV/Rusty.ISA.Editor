namespace Rusty.ISA.Editor;

/// <summary>
/// An instance of an instruction definition preview.
/// </summary>
public partial class InstructionPreviewInstance : PreviewInstance
{
    /* Public properties. */
    public new InstructionPreview Preview => base.Preview as InstructionPreview;

    /* Constructors. */
    public InstructionPreviewInstance(InstructionPreview preview) : base(preview) { }

    /* Public methods. */
    public override InstructionPreviewInstance Copy()
    {
        InstructionPreviewInstance copy = new(Preview);
        copy.Input.CopyFrom(Input);
        return copy;
    }

    /// <summary>
    /// Set a parameter preview.
    /// </summary>
    public void SetParameter(string id, ParameterPreviewInstance preview)
    {
        Input.SetValue(id, preview);
    }

    /// <summary>
    /// Set a compile rule preview.
    /// </summary>
    public void SetRule(string id, RulePreviewInstance preview)
    {
        Input.SetValue(id, preview);
    }
}