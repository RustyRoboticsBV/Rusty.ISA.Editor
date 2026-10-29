/// <summary>
/// An increment statement.
/// </summary>
public sealed class Increment : Statement
{
    /* Public properties. */
    public override bool SingleLine => true;

    public Id Name { get; set; }

    /* Constructors. */
    public Increment(Id name) => Name = name;

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        Name.Generate(w);
        w.Append("++;");

        return true;
    }
}