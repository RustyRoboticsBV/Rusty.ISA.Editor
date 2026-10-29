using System.Collections.Generic;

/// <summary>
/// A property generator.
/// </summary>
public sealed class Class : UserDefinedType
{
    /* Public properties. */
    public TypeName Base { get; set; }
    public List<TypeName> Interfaces { get; set; } = [];
    public List<IClassMember> Members { get; set; } = [];

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        // Summary.
        if (Summary != null)
        {
            Summary.Generate(w);
            w.Line();
        }

        // Header.
        if (Access.Generate(w))
            w.Space();

        if (Modifier.Generate(w))
            w.Space();

        w.Append("class ");

        Name.Generate(w);

        if (Base != null || Interfaces.Count > 0)
            w.Append(" : ");

        if (Base != null)
            Base.Generate(w);

        for (int i = 0; i < Interfaces.Count; i++)
        {
            if (Base != null || i > 0)
                w.Append(", ");
            Interfaces[i].Generate(w);
        }

        // Members.
        w.OpenBlock();
        foreach (SyntaxNode member in Members)
        {
            w.Line();
            member.Generate(w);
        }
        w.CloseBlock();

        return true;
    }
}