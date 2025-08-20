namespace Rusty.ISA.Editor;

/// <summary>
/// An instance of a compile rule preview.
/// </summary>
public abstract partial class RulePreviewInstance : PreviewInstance
{
    /* Public properties. */
    public new RulePreview Preview => base.Preview as RulePreview;

    /* Constructors. */
    public RulePreviewInstance(RulePreview preview) : base(preview) { }
}