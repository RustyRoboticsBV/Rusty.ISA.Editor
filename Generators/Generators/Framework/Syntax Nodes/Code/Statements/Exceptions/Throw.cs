/// <summary>
/// A throw statement generator.
/// </summary>
public class Throw : Statement
{
    /* Public properties. */
    public New Exception { get; set; }

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        w.Append("throw ");
        Exception.Generate(w);
        w.Append(';');

        return true;
    }
}