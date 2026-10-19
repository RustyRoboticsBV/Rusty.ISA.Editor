/// <summary>
/// A string value generator.
/// </summary>
public sealed class IString : Value
{
    public override bool SingleLine => !Value.Contains('\n');
    public string Value { get; set; }

    public IString(string v) => Value = v;

    public static implicit operator IString(string v) => new(v);

    public override bool Generate(CodeWriter w)
    {
        w.Append('$');
        w.Append('"');
        w.Append(Value.Replace("\"", "\\\""));
        w.Append('"');
        return true;
    }
}