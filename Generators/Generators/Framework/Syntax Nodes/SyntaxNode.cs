/// <summary>
/// A base class for all generators.
/// </summary>
public abstract class SyntaxNode
{
    /* Public properties. */
    /// <summary>
    /// Whether or not the generated output is single-line.
    /// </summary>
    public virtual bool SingleLine => !ToString().Contains('\n');

    /* Public methods. */
    /// <summary>
    /// Generate the code.
    /// </summary>
    public override string ToString()
    {
        CodeWriter w = new();
        Generate(w);
        return w.ToString();
    }

    /// <summary>
    /// Generate the code.
    /// </summary>
    public abstract bool Generate(CodeWriter w);
}