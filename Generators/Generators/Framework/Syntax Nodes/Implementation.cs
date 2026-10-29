using System.Collections.Generic;

/// <summary>
/// A function implementation generator.
/// </summary>
public sealed class Implementation : SyntaxNode
{
    /* Public properties. */
    public override bool SingleLine => IsEmpty || (Statements.Count == 1 && Statements[0].SingleLine);
    public bool IsEmpty => Statements == null || Statements.Count == 0;

    public List<Statement> Statements { get; set; } = [];

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        if (IsEmpty)
            w.Append(';');
        else if (SingleLine)
        {
            w.Append(" => ");
            if (Statements[0] is Return @return)
                @return.GenerateImplicit(w);
            else
                Statements[0].Generate(w);
        }
        else
        {
            w.OpenBlock();
            foreach (Statement statement in Statements)
            {
                w.Line();
                statement.Generate(w);
            }
            w.CloseBlock();
        }
        return true;
    }
}