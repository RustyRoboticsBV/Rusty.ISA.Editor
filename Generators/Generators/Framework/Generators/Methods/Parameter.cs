// <summary>
/// A parameter generator.
/// </summary>
public sealed class Parameter : Generator
{
    public Id Type { get; set; }
    public Id Name { get; set; }

    public Parameter(Id type, Id name)
    {
        Type = type;
        Name = name;
    }

    public override bool Generate(CodeWriter w)
    {
        Type.Generate(w);
        w.Space();
        Name.Generate(w);

        return true;
    }
}