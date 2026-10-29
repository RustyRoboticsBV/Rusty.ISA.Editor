using System.Collections.Generic;

/// <summary>
/// An if/else statement generator.
/// </summary>
public abstract class ConditionBlock : Statement
{
    /* Public properties. */
    public override bool SingleLine => false;
    public bool IsEmpty => Statements.Count == 0;

    public List<Statement> Statements { get; set; } = [];

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        // Header.
        GenerateHeader(w);

        // Contents.
        if (IsEmpty)
        {
            w.Append(" { }");
            return true;
        }

        if (Statements.Count == 1)
            w.Indent++;
        else
            w.OpenBlock();
        foreach (Statement s in Statements)
        {
            w.Line();
            s.Generate(w);
        }
        if (Statements.Count == 1)
            w.Indent--;
        else
            w.CloseBlock();

        return true;
    }

    /* Protected methods. */
    protected abstract void GenerateHeader(CodeWriter w);
}