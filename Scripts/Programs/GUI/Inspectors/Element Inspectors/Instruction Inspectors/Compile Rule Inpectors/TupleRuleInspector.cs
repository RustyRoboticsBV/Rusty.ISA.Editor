namespace Rusty.ISA.Editor;

public partial class TupleRuleInspector : RuleInspector
{
    /* Public properties. */
    public new TupleRule Rule => base.Rule as TupleRule;

    public new TupleRulePreviewInstance Preview
    {
        get => base.Preview as TupleRulePreviewInstance;
        protected set => base.Preview = value;
    }

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

        // Set preview.
        for (int i = 0; i < GetElementCount(); i++)
        {
            RuleInspector inspector = GetElementInspector(i);
            Preview.SetElement(i, inspector.Rule.ID, inspector.Preview);
        }
        Preview.SetCount(GetElementCount());
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

    /* Protected methods. */
    protected override void UpdatePreview() { }
}