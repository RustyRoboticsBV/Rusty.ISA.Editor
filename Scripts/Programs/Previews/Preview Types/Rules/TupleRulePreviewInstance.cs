namespace Rusty.ISA.Editor;

/// <summary>
/// An instance of a tuple rule preview.
/// </summary>
public partial class TupleRulePreviewInstance : RulePreviewInstance
{
    /* Public properties. */
    public new TupleRulePreview Preview => base.Preview as TupleRulePreview;

    /* Constructors. */
    public TupleRulePreviewInstance(TupleRulePreview preview) : base(preview) { }

    /* Public methods. */
    public override TupleRulePreviewInstance Copy()
    {
        TupleRulePreviewInstance copy = new(Preview);
        copy.Input.CopyFrom(Input);
        return copy;
    }

    /// <summary>
    /// Set an element preview.
    /// </summary>
    public void SetElement(int index, string id, PreviewInstance value)
    {
        Input.SetValue(RulePreview.Element + index, value);
        Input.SetValue(id, value);
    }

    /// <summary>
    /// Set the element count.
    /// </summary>
    public void SetCount(int value)
    {
        Input.SetValue(TupleRulePreview.Count, value);
    }
}