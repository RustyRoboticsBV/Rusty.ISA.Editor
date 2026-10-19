using System;

/// <summary>
/// A identifier generator.
/// </summary>
public sealed class Id : Value
{
    private string name;

    public override bool SingleLine => true;
    public string Name
    {
        get => name;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new Exception("Identifier names may not be empty.");
            if (value.Contains('\n'))
                throw new Exception("Identifier names may not be multiline.");
            name = value;
        }
    }

    public Id(string name) => Name = name;

    public static implicit operator Id(string name) => new(name);

    public override bool Generate(CodeWriter w)
    {
        w.Append(Name);
        return true;
    }
}