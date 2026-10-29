using System.Collections.Generic;

/// <summary>
/// A switch clause generator.
/// </summary>
public abstract class Clause : SyntaxNode
{
    /* Public properties. */
    public override bool SingleLine => false;

    public List<Statement> Statements { get; set; } = [];
    public bool OmitBreak { get; set; }

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        // Clause.
        GenerateClause(w);
        w.Append(':');

        w.Indent++;

        // Implementation.
        foreach (Statement s in Statements)
        {
            w.Line();
            s.Generate(w);
        }

        // Break.
        if (!OmitBreak && (Statements.Count == 0 || (Statements[^1] is not Return && Statements[^1] is not Throw)))
            w.Line("break;");

        w.Indent--;
        return true;
    }

    /* Protected methods. */
    protected abstract void GenerateClause(CodeWriter w);
}