namespace Rusty.ISA.Editor;

/// <summary>
/// An instruction rule preview.
/// </summary>
public class InstructionRulePreview : RulePreview
{
    /* Constructors. */
    public InstructionRulePreview(string code) : base(code) { }

    public InstructionRulePreview(InstructionSet set, InstructionRule rule) : this(rule.Preview)
    {
        InstructionDefinition definition = set[rule.Opcode];
        DefaultInput.SetValue(Element, PreviewDict.ForInstruction(set, definition)?.CreateInstance());
    }

    /* Public methods. */
    public override InstructionRulePreviewInstance CreateInstance()
    {
        return new(this);
    }
}