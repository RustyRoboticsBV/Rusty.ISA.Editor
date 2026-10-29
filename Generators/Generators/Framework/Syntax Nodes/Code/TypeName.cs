using System;

/// <summary>
/// A type name generator.
/// </summary>
public sealed class TypeName : SyntaxNode
{
    private string name;

    public override bool SingleLine => true;
    public string Name
    {
        get => name;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new Exception("Type names may not be empty.");
            if (value.Contains('\n'))
                throw new Exception("Type names may not be multiline.");
            name = value;
        }
    }

    public TypeName(string name) => Name = name;

    public static implicit operator TypeName(string name) => new(name);

    public override bool Generate(CodeWriter w)
    {
        w.Append(Name);
        return true;
    }
}