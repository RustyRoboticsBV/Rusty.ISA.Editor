using Godot;

namespace Rusty.ISA.Editor;

/// <summary>
/// A parameter preview.
/// </summary>
public class ParameterPreview : Preview
{
    /* Public constants. */
    public const string DisplayName = "name";
    public const string Value = "value";

    /* Private constants. */
    private const string DefaultCode = $"return [[{Value}]];";

    /* Constructors. */
    public ParameterPreview(string code) : base(code == "" ? DefaultCode : code) { }

    public ParameterPreview(Parameter parameter) : this(parameter.Preview)
    {
        DefaultInput.SetValue(DisplayName, parameter.DisplayName);
        DefaultInput.SetValue(Value, StringUtility.Serialize(ParameterUtility.GetDefaultValue(parameter)));
    }

    /* Public methods. */
    public override PreviewInstance CreateInstance()
    {
        return new ParameterPreviewInstance(this);
    }
}