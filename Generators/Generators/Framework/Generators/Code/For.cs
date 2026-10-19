/// <summary>
/// A for loop generator.
/// </summary>
public sealed class For : Statement
{
    public override bool SingleLine => false;
    public Statement Init { get; set; }
    public Statement Conditions { get; set; }
    public Statement Increment { get; set; }
    public Block Do { get; set; }

    public For(Statement init, Statement conditions, Statement increment, Block @do)
    {
        Init = init;
        Conditions = conditions;
        Increment = increment;
        Do = @do;
    }

    public override bool Generate(CodeWriter w)
    {
        w.Append("for (");
        Init.Generate(w);
        w.Append("; ");
        Conditions.Generate(w);
        w.Append("; ");
        Increment.Generate(w);
        w.Append(')');
        w.Line();
        Do.Generate(w);
        return true;
    }
}