namespace Rusty.ISA.Editor;

public abstract partial class RuleInspector : Inspector
{
    /* Public properties. */
    public CompileRule Rule { get; private set; }

    public RulePreviewInstance Preview { get; protected set; }

    /* Constructors. */
    public RuleInspector() : base() { }

    public RuleInspector(InstructionSet set, CompileRule rule) : base()
    {
        Rule = rule;

        Preview = PreviewDict.ForRule(rule).CreateInstance();
        Changed += UpdatePreview;
    }

    /* Public methods. */
    public override void CopyFrom(IGuiElement other)
    {
        base.CopyFrom(other);
        if (other is RuleInspector inspector)
            Rule = inspector.Rule;
    }

    /* Protected methods. */
    protected abstract void UpdatePreview();
}