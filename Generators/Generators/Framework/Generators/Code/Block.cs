/// <summary>
/// A statement block generator.
/// </summary>
public sealed class Block : Statement
{
    public override bool SingleLine => Statements.Length == 1 && Statements[0].SingleLine;
    public Statement[] Statements { get; set; }

    public Block(params Statement[] statements) => Statements = statements;

    public static implicit operator Block(Statement[] statements) => new(statements);

    public override bool Generate(CodeWriter w)
    {
        if (Statements.Length == 0)
            return false;

        Statements[0].Generate(w);
        for (int i = 1; i < Statements.Length; i++)
        {
            w.Line();
            Statements[i].Generate(w);
        }

        return true;
    }
}