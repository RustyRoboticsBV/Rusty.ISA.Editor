/// <summary>
/// A cast generator.
/// </summary>
public sealed class Cast : Value
{
    public override bool SingleLine => Value.SingleLine;
    public Id Type { get; set; }
    public Value Value { get; set; }

    public Cast(Id type, Value value)
    {
        Type = type;
        Value = value;
    }

    public override bool Generate(CodeWriter w)
    {
        w.Append('(');
        Type.Generate(w);
        w.Append(')');

        if (Value is BinaryValue)
            w.Append('(');
        Value.Generate(w);
        if (Value is BinaryValue)
            w.Append(')');
        return true;
    }
}