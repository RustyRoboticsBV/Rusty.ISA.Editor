namespace Rusty.ISA.Editor;

/// <summary>
/// An output dummy inspector.
/// </summary>
public partial class OutputInspector : ParameterInspector
{
    /* Public properties. */
    public new OutputParameter Parameter => base.Parameter as OutputParameter;

    public override object Value
    {
        get => "";
        set { }
    }

    /* Constructors. */
    public OutputInspector(InstructionInspector parent, OutputParameter output) : base(output)
    {
        Visible = false;
    }

    /* Public methods. */
    /// <summary>
    /// Update parameter previews.
    /// </summary>
    public void UpdatePreview(InstructionInspector instructionInspector)
    {
        for (int i = 0; i < instructionInspector.GetContentsCount(); i++)
        {
            if (instructionInspector.GetAt(i) is ParameterInspector parameter && parameter is not OutputInspector)
                Preview.SetValue(parameter.Value);
        }
    }
}