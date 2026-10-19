/// <summary>
/// A switch default case generator.
/// </summary>
public sealed class Default : Statement
{
    public override bool SingleLine => false;
    public Statement Do { get; set; }

    public Default(Statement @do) => Do = @do;

    public override bool Generate(CodeWriter w)
    {
        w.Append("default:");
        w.Indent++;
        w.Line();
        Do.Generate(w);
        if (Do is not Return && Do is not Throw)
            w.Line("break;");
        w.Indent--;
        return true;
    }
}