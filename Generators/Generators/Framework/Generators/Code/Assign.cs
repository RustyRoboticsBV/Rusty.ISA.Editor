public enum AssignOperator
{
    Normal,
    Add, Subtract, Multiply, Divide, Modulo,
    And, Or, Xor,
    BitshiftLeft, BitshiftRight, UnsignedShiftRight,
    NullCoalescing
}

/// <summary>
/// An assign statement generator.
/// </summary>
public sealed class Assign : Statement
{
    public override bool SingleLine => Value.SingleLine;
    public Id Name { get; set; }
    public AssignOperator Operator { get; set; } = AssignOperator.Normal;
    public Value Value { get; set; }

    public Assign(Id name, Value value)
    {
        Name = name;
        Value = value;
    }

    public Assign(Id name, AssignOperator @operator, Value value) : this(name, value) => Operator = @operator;

    public override bool Generate(CodeWriter w)
    {
        Name.Generate(w);
        w.Space();
        switch (Operator)
        {
            case AssignOperator.Add: w.Append('+'); break;
            case AssignOperator.Subtract: w.Append('-'); break;
            case AssignOperator.Multiply: w.Append('*'); break;
            case AssignOperator.Divide: w.Append('/'); break;
            case AssignOperator.Modulo: w.Append('%'); break;
            case AssignOperator.And: w.Append('&'); break;
            case AssignOperator.Or: w.Append('|'); break;
            case AssignOperator.Xor: w.Append('^'); break;
            case AssignOperator.BitshiftLeft: w.Append("<<"); break;
            case AssignOperator.BitshiftRight: w.Append(">>"); break;
            case AssignOperator.UnsignedShiftRight: w.Append(">>>"); break;
            case AssignOperator.NullCoalescing: w.Append("??"); break;
        }
        w.Append('=');
        w.Space();
        Value.Generate(w);
        w.Append(';');
        return true;
    }
}