/// <summary>
/// A base class for assignment statements.
/// </summary>
public abstract class Assignment : Statement
{
    /* Public properties. */
    public override bool SingleLine => Value.SingleLine;

    public Id Name { get; set; }
    public Value Value { get; set; }

    /* Protected properties. */
    protected abstract string Operator { get; }

    /* Public methods. */
    public sealed override bool Generate(CodeWriter w)
    {
        Name.Generate(w);

        w.Space();
        w.Append(Operator);
        w.Space();

        Value.Generate(w);
        w.Append(';');

        return true;
    }
}