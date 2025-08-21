namespace Rusty.ISA.Editor;

/// <summary>
/// A tuple rule preview.
/// </summary>
public class TupleRulePreview : RulePreview
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
    public TupleRulePreview(string code) : base(code == "" ? DefaultCode : code) { }

    public TupleRulePreview(InstructionSet set, TupleRule rule) : this(rule.Preview) { }

    /* Public methods. */
    public override TupleRulePreviewInstance CreateInstance()
    {
        return new(this);
    }
}