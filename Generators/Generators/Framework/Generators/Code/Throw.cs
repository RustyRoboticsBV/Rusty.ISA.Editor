/// <summary>
/// A throw statement generator.
/// </summary>
public sealed class Throw : Statement
{
    public override bool SingleLine => true;
    public Id Name { get; set; }
    public Value[] Arguments { get; set; }

    public Throw(Id name, params Value[] arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    public override bool Generate(CodeWriter w)
    {
        w.Append("throw new ");
        Name.Generate(w);
        w.Append("(");
        for (int i = 0; i < Arguments.Length; i++)
        {
            if (i > 0)
                w.Append(", ");
            Arguments[i].Generate(w);
        }
        w.Append(");");
        return true;
    }
}