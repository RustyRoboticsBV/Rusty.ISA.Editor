namespace Rusty.ISA.Editor;

/// <summary>
/// An editor node preview.
/// </summary>
public class EditorNodePreview : Preview
{
    /* Public constants. */
    public const string Main = "main";

    /* Private constants. */
    private const string DefaultCode = $"return [[{Main}]];";

    /* Constructors. */
    public EditorNodePreview(string code) : base(code == "" ? DefaultCode : code) { }

    public EditorNodePreview(InstructionDefinition instruction) : this(instruction.Preview)
    {
        // Create instruction preview.
        Preview main = PreviewDict.ForInstruction(instruction);

        // Copy its data.
        DefaultInput.CopyFrom(main.DefaultInput);

        // Add our own data.
        DefaultInput.SetValue(Main, main.CreateInstance());
    }

    /* Public methods. */
    public override EditorNodePreviewInstance CreateInstance()
    {
        return new(this);
    }
}