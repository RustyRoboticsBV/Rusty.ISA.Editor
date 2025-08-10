namespace Rusty.ISA.Editor;

public partial class RuleInspector : Inspector
{
    /* Constants. */
    private const string Title = "title+icon";

    /* Public properties. */
    public CompileRule Rule { get; private set; }

    /* Constructors. */
    public RuleInspector() : base() { }

    public RuleInspector(InstructionSet set, CompileRule rule) : this()
    {
        Rule = rule;
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        RuleInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public override void CopyFrom(IGuiElement other)
    {
        base.CopyFrom(other);
        if (other is RuleInspector inspector)
            Rule = inspector.Rule;
    }
}