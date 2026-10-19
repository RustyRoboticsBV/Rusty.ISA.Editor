using System.Collections.Generic;

/// <summary>
/// A property generator.
/// </summary>
public sealed class Method : Function
{
    public Access Access { get; set; } = new();
    public Modifier Modifier { get; set; } = new();
    public Id ReturnType { get; set; }
    public Id Name { get; set; }
    public List<Parameter> Parameters { get; set; } = [];

    protected override void GenerateHeader(CodeWriter w)
    {
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
    }
}