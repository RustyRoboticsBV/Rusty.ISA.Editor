using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An instance of a choice rule preview.
/// </summary>
public partial class ChoiceRulePreviewInstance : RulePreviewInstance
{
    /* Public properties. */
    public new ChoiceRulePreview Preview => base.Preview as ChoiceRulePreview;

    /* Constructors. */
    public ChoiceRulePreviewInstance(ChoiceRulePreview preview) : base(preview) { }

    /* Public methods. */
    public override ChoiceRulePreviewInstance Copy()
    {
        ChoiceRulePreviewInstance copy = new(Preview);
        copy.Input.CopyFrom(Input);
        return copy;
    }

    public void SetSelected(int value)
    {
        Input.SetValue(ChoiceRulePreview.Selected, value);
    }

    public void SetElement(PreviewInstance value)
    {
        Input.SetValue(RulePreview.Element, value);
    }
}