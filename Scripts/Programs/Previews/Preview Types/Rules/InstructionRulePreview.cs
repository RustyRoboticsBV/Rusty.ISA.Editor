namespace Rusty.ISA.Editor;

/// <summary>
/// An instruction rule preview.
/// </summary>
public class InstructionRulePreview : RulePreview
{
    /* Constructors. */
    public InstructionRulePreview(string code) : base(code) { }

    public InstructionRulePreview(InstructionRule rule) : this(rule.Preview)
    {
        DefaultInput.SetValue(Element, PreviewDict.ForInstructionRule(rule).CreateInstance());
    }

    /* Public methods. */
    public override InstructionRulePreviewInstance CreateInstance()
    {
        return new(this);
    }
}