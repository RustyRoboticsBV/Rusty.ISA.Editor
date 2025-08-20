namespace Rusty.ISA.Editor;

/// <summary>
/// A output preview.
/// </summary>
public class OutputPreview : Preview
{
    /* Public constants. */
    public const string Value = "value";

    /* Private constants. */
    private const string DefaultCode = $"return [[{Value}]];";

    /* Constructors. */
    public OutputPreview(string code) : base(code == "" ? DefaultCode : code) { }

    public OutputPreview(InstructionDefinition definition, string outputID)
        : this(definition.GetParameter(outputID).Preview)
    {
        OutputParameter output = definition.GetParameter(outputID) as OutputParameter;

        // Display name.
        DefaultInput.SetValue(DisplayName, output.DisplayName);

        // Parameters.
        for (int i = 0; i < definition.Parameters.Length; i++)
        {
            if (definition.Parameters[i] is not OutputParameter)
            {
                Parameter parameter = definition.Parameters[i];
                DefaultInput.SetValue(parameter.ID, PreviewDict.ForParameter(parameter).CreateInstance());
            }
        }
    }

    /* Public methods. */
    public override OutputPreviewInstance CreateInstance()
    {
        return new(this);
    }
}