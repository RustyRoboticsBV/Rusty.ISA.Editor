/// <summary>
/// A return statement generator.
/// </summary>
public sealed class Return : Statement
{
    /* Public properties. */
    public override bool SingleLine => Value.SingleLine;

    public Value Value { get; set; }

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        w.Append("return ");
        Value.Generate(w);
        w.Append(';');
        return true;
    }

    public bool GenerateImplicit(CodeWriter w)
    {
        Value.Generate(w);
        w.Append(';');
        return true;
    }
}