/// <summary>
/// A base class for function generators.
/// </summary>
public abstract class Function : Generator
{
    public Summary Summary { get; set; }
    public Statement Implementation { get; set; }
    public bool SingleLine => Summary == null && (Implementation == null || Implementation.SingleLine);

    public sealed override bool Generate(CodeWriter w)
    {
        if (Summary != null)
        {
            Summary.Generate(w);
            w.Line();
        }

        if (Implementation == null)
        {
            GenerateHeader(w);
            w.Append(';');
        }
        else if (Implementation.SingleLine)
        {
            GenerateHeader(w);
            w.Append(" => ");
            if (Implementation is Return @return)
                @return.GenerateImplicit(w);
            else
                Implementation.Generate(w);
        }
        else
        {
            GenerateHeader(w);
            w.OpenBlock();
            w.Line();
            Implementation.Generate(w);
            w.CloseBlock();
        }
        return true;
    }

    protected abstract void GenerateHeader(CodeWriter w);
}