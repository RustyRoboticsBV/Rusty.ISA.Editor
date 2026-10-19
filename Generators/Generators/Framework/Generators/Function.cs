/// <summary>
/// A base class for function generators.
/// </summary>
public abstract class Function : Generator, ISummarizable<Function>
{
    public Function This => this;
    public bool SingleLine
        => Summary == null && (Implementation.Length == 0 || Implementation.Length == 1 && Implementation[0].SingleLine);

    public Summary Summary { get; set; }
    public Statement[] Implementation { get; set; } = [];

    public Function() { }

    public Function(params Statement[] statement) => Implementation = statement;

    public sealed override bool Generate(CodeWriter w)
    {
        if (Summary != null)
        {
            Summary.Generate(w);
            w.Line();
        }

        if (Implementation.Length == 0)
        {
            GenerateHeader(w);
            w.Append(';');
        }
        else if (Implementation.Length == 1 && Implementation[0].SingleLine)
        {
            GenerateHeader(w);
            w.Append(" => ");
            if (Implementation[0] is Return @return)
                @return.GenerateImplicit(w);
            else
                Implementation[0].Generate(w);
        }
        else
        {
            GenerateHeader(w);
            w.OpenBlock();
            w.Line();
            foreach (Statement statement in Implementation)
            {
                statement.Generate(w);
            }
            w.CloseBlock();
        }
        return true;
    }

    protected abstract void GenerateHeader(CodeWriter w);
}