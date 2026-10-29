/// <summary>
/// A switch default case generator.
/// </summary>
public sealed class Default : Clause
{
    /* Protected methods. */
    protected override void GenerateClause(CodeWriter w) => w.Append("default");
}