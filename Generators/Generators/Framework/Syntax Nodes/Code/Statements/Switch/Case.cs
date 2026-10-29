/// <summary>
/// A switch case generator.
/// </summary>
public sealed class Case : Clause
{
    /* Public properties. */
    public Value Value { get; set; }

    /* Protected methods. */
    protected override void GenerateClause(CodeWriter w)
    {
        w.Append("case ");
        Value.Generate(w);
    }
}