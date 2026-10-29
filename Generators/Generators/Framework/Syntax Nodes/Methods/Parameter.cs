// <summary>
/// A parameter generator.
/// </summary>
public sealed class Parameter : SyntaxNode
{
    /* Public properties. */
    public TypeName Type { get; set; }
    public Id Name { get; set; }

    /* Constructors. */
    public Parameter(TypeName type, Id name)
    {
        Type = type;
        Name = name;
    }

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        Type.Generate(w);
        w.Space();
        Name.Generate(w);

        return true;
    }
}