using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An instance of an instruction rule preview.
/// </summary>
public partial class InstructionRulePreviewInstance : RulePreviewInstance
{
    /* Public properties. */
    public new InstructionRulePreview Preview => base.Preview as InstructionRulePreview;

    /* Constructors. */
    public InstructionRulePreviewInstance(InstructionRulePreview preview) : base(preview) { }

    /* Public methods. */
    public override InstructionRulePreviewInstance Copy()
    {
        InstructionRulePreviewInstance copy = new(Preview);
        copy.Input.CopyFrom(Input);
        return copy;
    }

    /// <summary>
    /// Set the instruction preview.
    /// </summary>
    public void SetElement(InstructionPreviewInstance value)
    {
        Input.SetValue(RulePreview.Element, value);
    }
}