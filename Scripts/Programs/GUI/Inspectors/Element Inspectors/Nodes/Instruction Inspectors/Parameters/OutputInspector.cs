using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// An output dummy inspector.
/// </summary>
public partial class OutputInspector : ParameterInspector
{
    /* Public properties. */
    public new OutputParameter Parameter => base.Parameter as OutputParameter;

    public new OutputPreviewInstance Preview
    {
        get => base.Preview as OutputPreviewInstance;
        set => base.Preview = value;
    }

    public override object Value
    {
        get => "";
        set { }
    }

    /* Constructors. */
    public OutputInspector(InstructionSet set, string opcode, string outputID)
        : base(set, opcode, outputID)
    {
        Visible = false;

        Preview = PreviewDict.ForOutput(set[opcode], outputID)?.CreateInstance();
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        OutputInspector copy = new(InstructionSet, InstructionDefinition.Opcode, Parameter.ID);
        copy.CopyFrom(this);
        return copy;
    }

    /// <summary>
    /// Update parameter previews.
    /// </summary>
    public void UpdatePreview(InstructionInspector instructionInspector)
    {
        for (int i = 0; i < instructionInspector.GetContentsCount(); i++)
        {
            if (instructionInspector.GetAt(i) is ParameterInspector parameter && parameter is not OutputInspector)
                Preview?.SetParameter(parameter.Parameter.ID, parameter.Preview);
        }
    }
}