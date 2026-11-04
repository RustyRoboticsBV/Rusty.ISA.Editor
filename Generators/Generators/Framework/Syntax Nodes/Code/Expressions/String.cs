/// <summary>
/// A string value generator.
/// </summary>
public sealed class String : Value
{
    public override bool SingleLine => !Value.Contains('\n');

    public string Value { get; set; }

    public override bool Generate(CodeWriter w)
    {
        w.Append("\"");
        w.Append(Value.Replace("\"", "\\\""));
        w.Append("\"");
        return true;
    }
}