/// <summary>
/// A function call generator.
/// </summary>
public class Call : Value
{
    /* Public properties. */
    public override bool SingleLine => Arguments.SingleLine;

    public Id Name { get; set; }
    public ArgumentList Arguments { get; set; }

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        Name.Generate(w);
        w.Append('(');
        Arguments.Generate(w);
        w.Append(')');

        return true;
    }
}