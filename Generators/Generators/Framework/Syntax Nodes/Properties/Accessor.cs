/// <summary>
/// A property accessor generator.
/// </summary>
public abstract class Accessor : SyntaxNode
{
    /* Public properties. */
    public Implementation Implementation { get; set; } = new();

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        GenerateHeader(w);
        Implementation.Generate(w);
        return true;
    }

    /* Protected methods. */
    protected abstract void GenerateHeader(CodeWriter w);
}