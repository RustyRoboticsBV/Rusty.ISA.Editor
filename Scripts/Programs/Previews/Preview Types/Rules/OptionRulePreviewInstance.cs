using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An instance of an option rule preview.
/// </summary>
public partial class OptionRulePreviewInstance : RulePreviewInstance
{
    /* Public properties. */
    public new OptionRulePreview Preview => base.Preview as OptionRulePreview;

    /* Constructors. */
    public OptionRulePreviewInstance(OptionRulePreview preview) : base(preview) { }

    /* Public methods. */
    public override OptionRulePreviewInstance Copy()
    {
        OptionRulePreviewInstance copy = new(Preview);
        copy.Input.CopyFrom(Input);
        return copy;
    }

    public void SetEnabled(bool value)
    {
        Input.SetValue(OptionRulePreview.Enabled, value);
    }

    public void SetElement(PreviewInstance value)
    {
        Input.SetValue(RulePreview.Element, value);
    }
}