/// <summary>
/// A character value generator.
/// </summary>
public sealed class Char : Value
{
    public override bool SingleLine => true;
    public char Value { get; set; }

    public Char(char v) => Value = v;

    public static implicit operator Char(char v) => new(v);

    public override bool Generate(CodeWriter w)
    {
        w.Append("'");
        w.Append(Value.ToString());
        w.Append("'");
        return true;
    }
}