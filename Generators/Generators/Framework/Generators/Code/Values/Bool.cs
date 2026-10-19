/// <summary>
/// A boolean value generator.
/// </summary>
public sealed class Bool : Value
{
    public override bool SingleLine => true;
    public bool Value { get; set; }

    public Bool(bool v) => Value = v;

    public static implicit operator Bool(bool v) => new(v);

    public override bool Generate(CodeWriter w)
    {
        if (Value)
            w.Append("true");
        else
            w.Append("false");
        return true;
    }
}