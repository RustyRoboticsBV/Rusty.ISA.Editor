/// <summary>
/// An integer value generator.
/// </summary>
public sealed class Int : Value
{
    /* Public properties. */
    public override bool SingleLine => true;

    public int Value { get; set; }

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        w.Append(Value.ToString());
        return true;
    }
}