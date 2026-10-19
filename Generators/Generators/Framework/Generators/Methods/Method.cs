using System.Collections.Generic;

/// <summary>
/// A property generator.
/// </summary>
public sealed class Method : Function, ISummarizable<Method>
{
    public new Method This => this;

    public Access Access { get; set; } = new();
    public Modifier Modifier { get; set; } = new();
    public Id ReturnType { get; set; }
    public Id Name { get; set; }
    public List<Parameter> Parameters { get; set; } = [];

    public Method() { }

    public Method(Access access, Modifier modifier, Id returnType, Id name)
    {
        Access = access;
        Modifier = modifier;
        ReturnType = returnType;
        Name = name;
    }

    public Method(Access access, Modifier modifier, Id returnType, Id name, params Parameter[] parameters)
        : this(access, modifier, returnType, name) => Parameters = new(parameters);

    public Method Parameter(Id type, Id name)
    {
        Parameters.Add(new(type, name));
        return this;
    }

    public Method Implement(Statement statement)
    {
        Implementation = statement;
        return this;
    }

    protected override void GenerateHeader(CodeWriter w)
    {
        if (Access.Generate(w))
            w.Space();
        if (Modifier.Generate(w))
            w.Space();
        ReturnType.Generate(w);
        w.Space();
        Name.Generate(w);
        w.Append('(');
        for (int i = 0; i < Parameters.Count; i++)
        {
            if (i > 0)
                w.Append(", ");
            Parameters[i].Generate(w);
        }
        w.Append(')');
    }
}