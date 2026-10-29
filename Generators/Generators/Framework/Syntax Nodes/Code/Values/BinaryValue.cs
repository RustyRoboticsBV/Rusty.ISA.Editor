using System;

public enum BinaryOperator
{
    Equal, NotEqual, Less, LessOrEqual, Greater, GreaterOrEqual,
    ConditionalAnd, ConditionalOr,
    Add, Subtract, Multiply, Divide, Modulo,
    BitwiseAnd, BitwiseOr, BitwiseXor,
    BitShiftLeft, BitShiftRight, BitShiftRightUnsigned,
    NullCoalescing
}

/// <summary>
/// A binary operation generator.
/// </summary>
public sealed class BinaryValue : Value
{
    public override bool SingleLine => Left.SingleLine && Right.SingleLine;
    public Value Left { get; set; }
    public BinaryOperator Operator { get; set; } = BinaryOperator.Add;
    public Value Right { get; set; }

    public BinaryValue(Value left, BinaryOperator @operator, Value right)
    {
        Left = left;
        Operator = @operator;
        Right = right;
    }

    public override bool Generate(CodeWriter w)
    {
        GenerateSide(Left, w);

        w.Space();
        switch (Operator)
        {
            case BinaryOperator.Equal: w.Append("=="); break;
            case BinaryOperator.NotEqual: w.Append("!="); break;
            case BinaryOperator.Less: w.Append('<'); break;
            case BinaryOperator.LessOrEqual: w.Append("<="); break;
            case BinaryOperator.Greater: w.Append('>'); break;
            case BinaryOperator.GreaterOrEqual: w.Append(">="); break;
            case BinaryOperator.ConditionalAnd: w.Append("&&"); break;
            case BinaryOperator.ConditionalOr: w.Append("||"); break;
            case BinaryOperator.Add: w.Append('+'); break;
            case BinaryOperator.Subtract: w.Append('-'); break;
            case BinaryOperator.Multiply: w.Append('*'); break;
            case BinaryOperator.Divide: w.Append('/'); break;
            case BinaryOperator.Modulo: w.Append('%'); break;
            case BinaryOperator.BitwiseOr: w.Append('|'); break;
            case BinaryOperator.BitwiseAnd: w.Append('&'); break;
            case BinaryOperator.BitwiseXor: w.Append('^'); break;
            case BinaryOperator.BitShiftLeft: w.Append("<<"); break;
            case BinaryOperator.BitShiftRight: w.Append(">>"); break;
            case BinaryOperator.BitShiftRightUnsigned: w.Append(">>>"); break;
            case BinaryOperator.NullCoalescing: w.Append("??"); break;
            default: throw new InvalidOperationException();
        }
        w.Space();

        GenerateSide(Right, w);

        return true;
    }

    private static void GenerateSide(Value value, CodeWriter w)
    {
        if (value is BinaryValue)
            w.Append('(');
        value.Generate(w);
        if (value is BinaryValue)
            w.Append(')');
    }
}