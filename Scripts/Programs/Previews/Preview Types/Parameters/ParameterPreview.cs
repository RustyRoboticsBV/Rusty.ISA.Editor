namespace Rusty.ISA.Editor;

/// <summary>
/// A parameter preview.
/// </summary>
public class ParameterPreview : Preview
{
    /* Public constants. */
    public const string Value = "value";

    /* Private constants. */
    private const string DefaultCode = $"return [[{Value}]];";

    /* Constructors. */
    public ParameterPreview(string code) : base(code == "" ? DefaultCode : code) { }

    public ParameterPreview(Parameter parameter) : this(parameter.Preview)
    {
        // Display name.
        DefaultInput.SetValue(DisplayName, parameter.DisplayName);

        // Value.
        string value = StringUtility.Serialize(ParameterUtility.GetDefaultValue(parameter));
        DefaultInput.SetValue(Value, value);
    }

    /* Public methods. */
    public override ParameterPreviewInstance CreateInstance()
    {
        return new(this);
    }
}