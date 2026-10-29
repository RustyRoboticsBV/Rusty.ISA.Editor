/// <summary>
/// An if statement generator.
/// </summary>
public sealed class If : ConditionBlock
{
    /* Public properties. */
    public override bool SingleLine => false;

    public Value Condition { get; set; }

    /* Protected methods. */
    protected override void GenerateHeader(CodeWriter w)
    {
        w.Append("if (");
        Condition.Generate(w);
        w.Append(')');
    }
}