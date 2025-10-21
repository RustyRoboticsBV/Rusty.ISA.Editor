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
    }

    /* Public methods. */
    public override void CopyFrom(IGuiElement other)
    {
        DisablePreview();

        // Base resource inspector copy.
        base.CopyFrom(other);

        // Copy rule.
        if (other is RuleInspector inspector)
            Rule = inspector.Rule;

        // Enable preview.
        EnablePreview();
    }

    /* Protected methods. */
    protected override void UpdatePreview()
    {
        if (Preview == null)
            Preview = PreviewDict.ForRule(InstructionSet, Rule)?.CreateInstance();
    }
}