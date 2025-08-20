using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A parameter preview.
/// </summary>
public partial class ParameterPreviewInstance : PreviewInstance
{
    /* Public properties. */
    public new ParameterPreview Preview => base.Preview as ParameterPreview;

    /* Constructors. */
    public ParameterPreviewInstance(ParameterPreview preview) : base(preview) { }

    /* Public methods. */
    public override ParameterPreviewInstance Copy()
    {
        ParameterPreviewInstance copy = new(Preview);
        copy.Input.CopyFrom(Input);
        return copy;
    }

    public void SetDisplayName(string value)
    {
        Input.SetValue(ParameterPreview.DisplayName, value);
    }

    public void SetValue(object value)
    {
        Input.SetValue(ParameterPreview.Value, StringUtility.Serialize(value));
    }
}