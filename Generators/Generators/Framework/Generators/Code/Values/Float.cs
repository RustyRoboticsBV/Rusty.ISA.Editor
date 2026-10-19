/// <summary>
/// A floating-point value generator.
/// </summary>
public sealed class Float : Value
{
    public override bool SingleLine => true;
    public decimal Value { get; set; }

    public Float(decimal v) => Value = v;

    public static implicit operator Float(decimal v) => new(v);

    public override bool Generate(CodeWriter w)
    {
        w.Append(Value.ToString());
        return true;
    }
}