using System.Collections.Generic;

/// <summary>
/// A property generator.
/// </summary>
public sealed class Class : Generator
{
    public Access Access { get; set; } = new();
    public Modifier Modifier { get; set; } = new();
    public Id Name { get; set; }
    public Id Base { get; set; }
    public List<Id> Interfaces { get; set; } = [];
    public List<Generator> Members { get; set; } = [];

    public override bool Generate(CodeWriter w)
    {
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

        w.OpenBlock();
        foreach (Generator member in Members)
        {
            w.Line();
            member.Generate(w);
        }
        w.CloseBlock();

        return true;
    }
}