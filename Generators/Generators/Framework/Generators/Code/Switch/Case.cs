/// <summary>
/// A switch case generator.
/// </summary>
public sealed class Case : Statement
{
    public override bool SingleLine => false;
    public Value Value { get; set; }
    public Statement Do { get; set; }

    public Case(Value value, Statement @do)
    {
        Value = value;
        Do = @do;
    }

    public override bool Generate(CodeWriter w)
    {
        w.Append("case ");
        Value.Generate(w);
        w.Append(':');
        w.Indent++;
        w.Line();
        Do.Generate(w);
        if (Do is not Return && Do is not Throw)
            w.Line("break;");
        w.Indent--;
        return true;
    }
}