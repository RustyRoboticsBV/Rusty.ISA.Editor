/// <summary>
/// A return statement generator.
/// </summary>
public sealed class Return : Statement
{
    public override bool SingleLine => Value.SingleLine;
    public Value Value { get; set; }

    public Return(Value value) => Value = value;

    public static implicit operator Return(Value v) => new(v);

    public override bool Generate(CodeWriter w)
    {
        w.Append("return ");
        Value.Generate(w);
        w.Append(';');
        return true;
    }

    public bool GenerateImplicit(CodeWriter w)
    {
        Value.Generate(w);
        w.Append(';');
        return true;
    }
}