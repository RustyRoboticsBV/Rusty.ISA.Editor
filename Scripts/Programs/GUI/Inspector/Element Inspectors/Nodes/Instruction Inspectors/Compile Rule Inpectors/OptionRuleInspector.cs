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

        // Enable preview.
        EnablePreview();
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        OptionRuleInspector copy = new(InstructionSet, Rule);
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
        // Init.
        base.UpdatePreview();

        // Update.
        if (Preview != null)
        {
            // Enabled.
            Preview.SetEnabled(GetEnabled());

            // Element.
            Preview.SetElement(GetEnabled(), GetChildRule().Preview);
        }
    }
}