namespace Rusty.ISA.Editor;

/// <summary>
/// An instance of an editor node preview.
/// </summary>
public partial class EditorNodePreviewInstance : PreviewInstance
{
    /* Public properties. */
    public new EditorNodePreview Preview => base.Preview as EditorNodePreview;

    /* Constructors. */
    public EditorNodePreviewInstance(EditorNodePreview preview) : base(preview) { }

    /* Public methods. */
    public override EditorNodePreviewInstance Copy()
    {
        EditorNodePreviewInstance copy = new(Preview);
        copy.Input.CopyFrom(Input);
        return copy;
    }

    /// <summary>
    /// Set the main instruction preview.
    /// </summary>
    public void SetMain(InstructionPreviewInstance main)
    {
        Input.SetValue(EditorNodePreview.Main, main);
    }

    /// <summary>
    /// Set a parameter preview.
    /// </summary>
    public void SetParameter(string id, ParameterPreviewInstance preview)
    {
        Input.SetValue(id, preview);
    }

    /// <summary>
    /// Set a compile rule preview.
    /// </summary>
    public void SetRule(string id, RulePreviewInstance preview)
    {
        Input.SetValue(id, preview);
    }
}