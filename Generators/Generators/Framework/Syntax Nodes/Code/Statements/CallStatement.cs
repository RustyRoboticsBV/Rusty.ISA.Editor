/// <summary>
/// A function call statement generator.
/// </summary>
public sealed class CallStatement : Statement
{
    /* Public properties. */
    public override bool SingleLine => Call.SingleLine;

    public Call Call { get; set; }

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        Call.Generate(w);
        w.Append(';');

        return true;
    }
}