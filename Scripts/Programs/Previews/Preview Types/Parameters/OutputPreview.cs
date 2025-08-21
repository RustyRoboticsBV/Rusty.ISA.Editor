namespace Rusty.ISA.Editor;

/// <summary>
/// A output preview.
/// </summary>
public class OutputPreview : ParameterPreview
{
    /* Private constants. */
    private const string DefaultCode = $"return [[{DisplayName}]];";

    /* Constructors. */
    public OutputPreview(string code) : base(code == "" ? DefaultCode : code) { }

    public OutputPreview(InstructionDefinition definition, string outputID)
        : this(definition.GetParameter(outputID).Preview)
    {
        Parameter output = definition.GetParameter(outputID);

        // Display name.
        DefaultInput.SetValue(DisplayName, output.DisplayName);

        // Non-output parameters values.
        for (int i = 0; i < definition.Parameters.Length; i++)
        {
            if (definition.Parameters[i] is not OutputParameter)
            {
                Parameter parameter = definition.Parameters[i];
                DefaultInput.SetValue(parameter.ID, PreviewDict.ForParameter(definition, parameter.ID)?.CreateInstance());
            }
        }
    }

    /* Public methods. */
    public override OutputPreviewInstance CreateInstance()
    {
        return new(this);
    }
}