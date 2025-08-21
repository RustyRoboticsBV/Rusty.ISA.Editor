namespace Rusty.ISA.Editor;

public abstract partial class RuleInspector : ResourceInspector
{
    /* Public properties. */
    public CompileRule Rule { get; private set; }

    public new RulePreviewInstance Preview
    {
        get => base.Preview as RulePreviewInstance;
        protected set => base.Preview = value;
    }

    /* Constructors. */
    public RuleInspector(InstructionSet set, CompileRule rule) : base(set)
    {
        Rule = rule;

        Preview = PreviewDict.ForRule(set, rule)?.CreateInstance();
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