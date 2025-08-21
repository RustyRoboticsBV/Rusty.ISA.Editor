namespace Rusty.ISA.Editor;

public partial class InstructionRuleInspector : RuleInspector
{
    /* Constants. */
    private const string Instruction = "instruction";

    public new InstructionRulePreviewInstance Preview
    {
        get => base.Preview as InstructionRulePreviewInstance;
        protected set => base.Preview = value;
    }

    /* Public properties. */
    public new InstructionRule Rule => base.Rule as InstructionRule;
    public InstructionDefinition Definition => GetInstructionInspector().Definition;

    /* Constructors. */
    public InstructionRuleInspector(InstructionSet set, InstructionRule rule)
        : base(set, rule)
    {
        // Create instruction inspector.
        InstructionInspector instruction = new(set, rule.Opcode);
        Add(Instruction, instruction);

        // Set preview.
        Preview?.SetElement(instruction.Preview);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        InstructionRuleInspector copy = new(InstructionSet, Rule);
        copy.CopyFrom(this);
        return copy;
    }

    public InstructionInspector GetInstructionInspector()
    {
        return GetAt(Instruction) as InstructionInspector;
    }

    /* Protected methods. */
    protected override void UpdatePreview() { }
}