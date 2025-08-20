namespace Rusty.ISA.Editor;

/// <summary>
/// A choice rule preview.
/// </summary>
public class ChoiceRulePreview : RulePreview
{
    /* Public constants. */
    public const string Selected = "selected";

    /* Constructors. */
    public ChoiceRulePreview(string code) : base(code) { }

    public ChoiceRulePreview(ChoiceRule rule) : this(rule.Preview)
    {
        DefaultInput.SetValue(Selected, -1);
        DefaultInput.SetValue(Element, "");
    }

    /* Public methods. */
    public override ChoiceRulePreviewInstance CreateInstance()
    {
        return new(this);
    }
}