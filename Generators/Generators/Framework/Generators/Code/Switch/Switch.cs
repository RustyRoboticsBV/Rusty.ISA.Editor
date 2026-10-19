using System.Collections.Generic;

/// <summary>
/// A switch case generator.
/// </summary>
public sealed class Switch : Statement
{
    public override bool SingleLine => false;
    public Value Value { get; set; }
    public List<Case> Cases { get; set; } = [];
    public Default Default { get; set; }

    public Switch(Value value) => Value = value;

    public override bool Generate(CodeWriter w)
    {
        w.Append("switch (");
        Value.Generate(w);
        w.Append(')');
        w.OpenBlock();
        foreach (Case @case in Cases)
        {
            w.Line();
            @case.Generate(w);
        }
        if (Default != null)
        {
            w.Line();
            Default.Generate(w);
        }
        w.CloseBlock();
        return true;
    }
}