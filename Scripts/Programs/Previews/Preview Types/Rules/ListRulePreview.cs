namespace Rusty.ISA.Editor;

/// <summary>
/// A list rule preview.
/// </summary>
public class ListRulePreview : RulePreview
{
    /* Public constants. */
    public const string Count = "count";

    /* Private constants. */
    private const string DefaultCode =
$"var result : String = \"\";"
+ $"\nfor i in [[{Count}]]:"
+ $"\n\tif i > 0:"
+ $"\n\t\tresult += '\\n'"
+ $"\n\tresult += [[{Element}i]];"
+ $"\nreturn result;";

    /* Constructors. */
    public ListRulePreview(string code) : base(code == "" ? DefaultCode : code) { }

    public ListRulePreview(InstructionSet set, ListRule rule) : this(rule.Preview) { }

    /* Public methods. */
    public override ListRulePreviewInstance CreateInstance()
    {
        return new(this);
    }
}