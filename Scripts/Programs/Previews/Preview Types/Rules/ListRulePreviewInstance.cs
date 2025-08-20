namespace Rusty.ISA.Editor;

/// <summary>
/// An instance of a list rule preview.
/// </summary>
public partial class ListRulePreviewInstance : RulePreviewInstance
{
    /* Public properties. */
    public new ListRulePreview Preview => base.Preview as ListRulePreview;

    /* Constructors. */
    public ListRulePreviewInstance(ListRulePreview preview) : base(preview) { }

    /* Public methods. */
    public override ListRulePreviewInstance Copy()
    {
        ListRulePreviewInstance copy = new(Preview);
        copy.Input.CopyFrom(Input);
        return copy;
    }

    /// <summary>
    /// Set an element preview.
    /// </summary>
    public void SetElement(int index, PreviewInstance value)
    {
        Input.SetValue(RulePreview.Element + index, value);
    }

    /// <summary>
    /// Set the element count.
    /// </summary>
    public void SetCount(int value)
    {
        Input.SetValue(ListRulePreview.Count, value);
    }
}