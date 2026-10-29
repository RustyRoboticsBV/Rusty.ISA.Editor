/// <summary>
/// An || operator expression generator.
/// </summary>
public sealed class Or : Value
{
    /* Public properties. */
    public override bool SingleLine => Left.SingleLine && Right.SingleLine;

    public Value Left { get; set; }
    public Value Right { get; set; }

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        Left.Generate(w);
        w.Append(" || ");
        Right.Generate(w);

        return true;
    }
}