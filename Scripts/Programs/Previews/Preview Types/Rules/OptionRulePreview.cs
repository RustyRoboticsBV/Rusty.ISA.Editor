namespace Rusty.ISA.Editor;

/// <summary>
/// A option rule preview.
/// </summary>
public class OptionRulePreview : RulePreview
{
    /* Public constants. */
    public const string Enabled = "enabled";

    /* Constructors. */
    public OptionRulePreview(string code) : base(code) { }

    public OptionRulePreview(InstructionSet set, OptionRule rule) : this(rule.Preview)
    {
        DefaultInput.SetValue(Enabled, false);
        DefaultInput.SetValue(Element, "");
    }

    /* Public methods. */
    public override OptionRulePreviewInstance CreateInstance()
    {
        return new(this);
    }
}