namespace Rusty.ISA.Editor;

/// <summary>
/// A compile rule preview.
/// </summary>
public abstract class RulePreview : Preview
{
    /* Public constants. */
    public const string Element = "element";

    /* Private constants. */
    private const string DefaultCode = $"return [[{Element}]];";

    /* Constructors. */
    public RulePreview(string code) : base(code == "" ? DefaultCode : code) { }

    public RulePreview(CompileRule rule) : this(rule.Preview)
    {
        DefaultInput.SetValue(DisplayName, rule.DisplayName);
    }

    /* Public methods. */
    public abstract override RulePreviewInstance CreateInstance();
}