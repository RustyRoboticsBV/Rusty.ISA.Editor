namespace Rusty.ISA.Editor;

public partial class TupleRuleInspector : RuleInspector
{
    /* Public properties. */
    public new TupleRule Rule => base.Rule as TupleRule;

    /* Constructors. */
    public TupleRuleInspector() : base() { }

    public TupleRuleInspector(InstructionSet set, TupleRule rule)
        : base(set, rule)
    {
        // Create element inspectors.
        foreach (CompileRule type in rule.Types)
        {
            RuleInspector childInspector = RuleInspectorFactory.Create(set, type);
            Add(GetContentsCount().ToString(), childInspector);
        }
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        TupleRuleInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public int GetElementCount()
    {
        return GetContentsCount();
    }

    public RuleInspector GetElementInspector(int index)
    {
        return GetAt(index) as RuleInspector;
    }
}