using System.Collections.Generic;

/// <summary>
/// A property generator.
/// </summary>
public sealed class Method : SyntaxNode, IClassMember
{
    /* Public properties. */
    public Summary Summary { get; set; }
    public Access Access { get; set; } = AccessType.Public;
    public Modifier Modifier { get; set; } = ModifierType.None;
    public TypeName ReturnType { get; set; }
    public Id Name { get; set; }
    public List<Parameter> Parameters { get; set; } = [];
    public Implementation Implementation { get; set; } = new();

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        if (Summary != null)
        {
            Summary.Generate(w);
            w.Line();
        }
        if (Access.Generate(w))
            w.Space();
        if (Modifier.Generate(w))
            w.Space();
        ReturnType.Generate(w);
        w.Space();
        Name.Generate(w);
        w.Append('(');
        for (int i = 0; i < Parameters.Count; i++)
        {
            if (i > 0)
                w.Append(", ");
            Parameters[i].Generate(w);
        }
        w.Append(')');
        Implementation.Generate(w);
        return true;
    }
}