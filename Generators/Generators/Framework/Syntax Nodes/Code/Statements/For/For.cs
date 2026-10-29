using System;
using System.Collections.Generic;

/// <summary>
/// A simple for loop generator.
/// </summary>
public sealed class For : SyntaxNode
{
    /* Public properties. */
    public override bool SingleLine => false;

    public TypeName Type { get; set; }
    public Id Name { get; set; }
    public Value From { get; set; }
    public Value To { get; set; }
    public List<Statement> Statements { get; set; } = [];

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        // Header.
        w.Append("for (");

        Type.Generate(w);
        w.Append(' ');
        Name.Generate(w);
        w.Append(" = ");
        From.Generate(w);
        w.Append(';');

        Name.Generate(w);
        w.Append(" < ");
        To.Generate(w);
        w.Append(';');

        Name.Generate(w);
        w.Append("++");
        w.Append(';');

        // Contents.
        w.OpenBlock();
        foreach (Statement s in Statements)
        {
            w.Line();
            s.Generate(w);
        }
        w.CloseBlock();

        return true;
    }
}