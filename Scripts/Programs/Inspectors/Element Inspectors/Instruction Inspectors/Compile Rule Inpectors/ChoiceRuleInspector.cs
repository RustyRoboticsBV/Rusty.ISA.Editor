namespace Rusty.ISA.Editor;

public partial class ChoiceRuleInspector : RuleInspector
{
    /* Public properties. */
    public new ChoiceRule Rule => base.Rule as ChoiceRule;

    /* Constructors. */
    public ChoiceRuleInspector() : base()
    {
        // Create check-box.
        CheckBoxBorderContainer list = new();
        ReplaceContainer(list);
    }

    public ChoiceRuleInspector(InstructionSet set, ChoiceRule rule)
        : base(set, rule)
    {
        // Create dropdown border.
        DropdownBorderContainer dropdown = new();
        dropdown.DropdownText = rule.DisplayName;
        ReplaceContainer(dropdown);

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
        ChoiceRuleInspector copy = new();
        copy.CopyFrom(this);
        return copy;
    }

    public int GetSelectedIndex()
    {
        return (ContentsContainer as DropdownBorderContainer).SelectedOption;
    }

    public RuleInspector GetSelectedElement()
    {
        return GetAt(GetSelectedIndex()) as RuleInspector;
    }
}