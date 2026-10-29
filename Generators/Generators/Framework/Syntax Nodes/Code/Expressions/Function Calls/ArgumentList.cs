using System.Collections.Generic;

/// <summary>
/// An argument list generator.
/// </summary>
public class ArgumentList : SyntaxNode
{
    /* Public properties. */
    public override bool SingleLine
    {
        get
        {
            foreach (Value argument in Arguments)
            {
                if (!argument.SingleLine)
                    return false;
            }
            return true;
        }
    }
    public List<Value> Arguments { get; set; } = new();

    /* Constructors. */
    public ArgumentList() { }
    public ArgumentList(Value[] arguments) => Arguments = new(arguments);

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        for (int i = 0; i < Arguments.Count; i++)
        {
            if (i > 0)
                w.Append(", ");
            Arguments[i].Generate(w);
        }
        return true;
    }
}