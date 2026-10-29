/// <summary>
/// A cast generator.
/// </summary>
public sealed class Cast : Value
{
    public override bool SingleLine => Value.SingleLine;

    public TypeName Type { get; set; }
    public Value Value { get; set; }

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