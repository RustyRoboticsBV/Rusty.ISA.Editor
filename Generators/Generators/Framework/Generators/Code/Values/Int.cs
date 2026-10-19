/// <summary>
/// An integer value generator.
/// </summary>
public sealed class Int : Value
{
    public override bool SingleLine => true;
    public int Value { get; set; }

    public Int(int v) => Value = v;

    public static implicit operator Int(int v) => new(v);

    public override bool Generate(CodeWriter w)
    {
        w.Append(Value.ToString());
        return true;
    }
}