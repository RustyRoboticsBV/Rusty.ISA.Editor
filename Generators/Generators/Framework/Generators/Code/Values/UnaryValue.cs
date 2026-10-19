using System;

public enum UnaryOperator
{
    Plus,
    Negate,
    LogicalNot,
    BitwiseNot,
}

/// <summary>
/// An unary operation generator.
/// </summary>
public sealed class UnaryValue : Value
{
    public override bool SingleLine => Operand.SingleLine;
    public Value Operand { get; set; }
    public UnaryOperator Operator { get; set; } = UnaryOperator.Negate;

    public UnaryValue(UnaryOperator @operator, Value operand)
    {
        Operator = @operator;
        Operand = operand;
    }

    public override bool Generate(CodeWriter w)
    {
        switch (Operator)
        {
            case UnaryOperator.Plus:
                w.Append('+');
                break;
            case UnaryOperator.Negate:
                w.Append('-');
                break;
            case UnaryOperator.LogicalNot:
                w.Append('!');
                break;
            case UnaryOperator.BitwiseNot:
                w.Append('~');
                break;
            default: throw new InvalidOperationException();
        }

        if (Operand is BinaryValue || Operand is UnaryValue)
            w.Append('(');
        Operand.Generate(w);
        if (Operand is BinaryValue || Operand is UnaryValue)
            w.Append(')');

        return true;
    }
}