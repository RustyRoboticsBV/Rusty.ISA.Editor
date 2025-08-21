using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An instance of an output parameter preview.
/// </summary>
public partial class OutputPreviewInstance : ParameterPreviewInstance
{
    /* Public properties. */
    public new OutputPreview Preview => base.Preview as OutputPreview;

    /* Constructors. */
    public OutputPreviewInstance(OutputPreview preview) : base(preview) { }

    /* Public methods. */
    public override OutputPreviewInstance Copy()
    {
        OutputPreviewInstance copy = new(Preview);
        copy.Input.CopyFrom(Input);
        return copy;
    }

    /// <summary>
    /// Set a parameter preview.
    /// </summary>
    public void SetParameter(string id, ParameterPreviewInstance preview)
    {
        Input.SetValue(id, preview);
    }
}