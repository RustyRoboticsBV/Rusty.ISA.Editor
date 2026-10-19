/// <summary>
/// A base class for all generators.
/// </summary>
public abstract class Generator
{
    public override string ToString()
    {
        CodeWriter w = new();
        Generate(w);
        return w.ToString();
    }

    public abstract bool Generate(CodeWriter w);
}