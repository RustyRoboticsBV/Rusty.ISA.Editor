using System.Collections.Generic;

/// <summary>
/// A statement block generator.
/// </summary>
public sealed class Block : Statement
{
    public override bool SingleLine => Statements.Count == 1 && Statements[0].SingleLine;
    public List<Statement> Statements { get; set; }

    public Block(params Statement[] statements) => Statements = new(statements);

    public static implicit operator Block(Statement[] statements) => new(statements);

    public override bool Generate(CodeWriter w)
    {
        if (Statements.Count == 0)
            return false;

        if (Statements.Count > 1)
            w.OpenBlock();

        Statements[0].Generate(w);
        for (int i = 1; i < Statements.Count; i++)
        {
            w.Line();
            Statements[i].Generate(w);
        }

        if (Statements.Count > 1)
            w.CloseBlock();

        return true;
    }
}