namespace Rusty.ISA.Editor;

public partial class OptionRuleInspector : RuleInspector
{
    /* Public properties. */
    public new OptionRule Rule => base.Rule as OptionRule;

    public new OptionRulePreviewInstance Preview
    {
        get => base.Preview as OptionRulePreviewInstance;
        protected set => base.Preview = value;
    }

    /* Constructors. */
    public OptionRuleInspector() : base()
    {
        // Create check-box.
        CheckBoxBorderContainer list = new();
        ReplaceContainer(list);
    }

    public OptionRuleInspector(InstructionSet set, OptionRule rule)
        : base(set, rule)
    {
        // Create check-box.
        CheckBoxBorderContainer checkBox = new();
        checkBox.CheckBoxText = rule.DisplayName;
        ReplaceContainer(checkBox);

        // Create child rule inspector.
        RuleInspector childInspector = RuleInspectorFactory.Create(set, rule.Type);
        Add("target", childInspector);
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        OptionRuleInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public bool GetEnabled()
    {
        return (ContentsContainer as CheckBoxBorderContainer).CheckBoxEnabled;
    }

    public RuleInspector GetChildRule()
    {
        return GetAt(0) as RuleInspector;
    }

    /* Protected methods. */
    protected override void UpdatePreview()
    {
        Preview.SetEnabled(GetEnabled());
        Preview.SetElement(GetChildRule().Preview);
    }
}