namespace Rusty.ISA.Editor;

/// <summary>
/// An instruction preview.
/// </summary>
public class InstructionPreview : Preview
{
    /* Public constants. */
    public const string Value = "value";

    /* Private constants. */
    private const string DefaultCode = $"return [[{Value}]];";

    /* Constructors. */
    public InstructionPreview(string code) : base(code == "" ? DefaultCode : code) { }

    public InstructionPreview(InstructionSet set, InstructionDefinition instruction) : this(instruction.Preview)
    {
        // Display name.
        DefaultInput.SetValue(DisplayName, instruction.DisplayName);

        // Parameters.
        for (int i = 0; i < instruction.Parameters.Length; i++)
        {
            Parameter parameter = instruction.Parameters[i];
            if (parameter is OutputParameter)
                DefaultInput.SetValue(parameter.ID, PreviewDict.ForOutput(instruction, parameter.ID)?.CreateInstance());
            else
                DefaultInput.SetValue(parameter.ID, PreviewDict.ForParameter(instruction.Parameters[i])?.CreateInstance());
        }

        // Compile rules.
        for (int i = 0; i < instruction.PreInstructions.Length; i++)
        {
            CompileRule rule = instruction.PreInstructions[i];
            DefaultInput.SetValue(rule.ID, PreviewDict.ForRule(set, rule)?.CreateInstance());
        }
        for (int i = 0; i < instruction.PostInstructions.Length; i++)
        {
            CompileRule rule = instruction.PostInstructions[i];
            DefaultInput.SetValue(rule.ID, PreviewDict.ForRule(set, rule)?.CreateInstance());
        }
    }

    /* Public methods. */
    public override InstructionPreviewInstance CreateInstance()
    {
        return new(this);
    }
}