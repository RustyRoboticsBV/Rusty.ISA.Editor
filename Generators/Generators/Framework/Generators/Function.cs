/// <summary>
/// A base class for function generators.
/// </summary>
public abstract class Function : Generator
{
    public bool SingleLine => Implementation == null || Implementation.SingleLine;

    public Implementation Implementation { get; set; }

    public override bool Generate(CodeWriter w)
    {
        if (Implementation == null)
        {
            GenerateHeader(w);
            w.Append(';');
        }
        else if (Implementation.SingleLine)
        {
            GenerateHeader(w);
            w.Append(" => ");
            Implementation.Generate(w);
            w.Append(';');
        }
        else
        {
            GenerateHeader(w);
            w.OpenBlock();
            Implementation.Generate(w);
            w.CloseBlock();
        }
        return true;
    }

    protected abstract void GenerateHeader(CodeWriter w);
}