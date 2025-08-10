namespace Rusty.ISA.Editor;

public partial class InstructionRuleInspector : RuleInspector
{
    /* Constants. */
    private const string Instruction = "instruction";

    /* Public properties. */
    public new InstructionRule Rule => base.Rule as InstructionRule;
    public InstructionDefinition Definition => GetInstructionInspector().Definition;

    /* Constructors. */
    public InstructionRuleInspector() : base() { }

    public InstructionRuleInspector(InstructionSet set, InstructionRule rule)
        : base(set, rule)
    {
        InstructionInspector instruction = new(set, set[rule.Opcode]);
        Add(Instruction, instruction);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        InstructionRuleInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public InstructionInspector GetInstructionInspector()
    {
        return GetAt(Instruction) as InstructionInspector;
    }
}