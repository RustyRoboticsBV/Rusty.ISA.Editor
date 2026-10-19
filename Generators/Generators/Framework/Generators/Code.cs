/// <summary>
/// A generic code generator.
/// </summary>
public sealed class Code : Statement
{
    public override bool SingleLine => !CodeString.Contains('\n');
    public string CodeString { get; set; }

    public Code(string code) => CodeString = code;

    public static implicit operator Code(string code) => new Code(code);

    public override bool Generate(CodeWriter w)
    {
        if (SingleLine)
            w.Append(CodeString);
        else
        {
            string[] lines = CodeString.Split('\n');
            foreach (string line in lines)
            {
                w.Line(line);
            }
        }
        return CodeString.Length > 0;
    }
}