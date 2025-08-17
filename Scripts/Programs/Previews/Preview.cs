using Godot;
using System;
using System.Text;

namespace Rusty.ISA.Editor;

/// <summary>
/// A class that can evaluate an object preview.
/// </summary>
public class Preview
{
    /* Fields. */
    private const string SkeletonCode = "func eval(_input) -> String:\n    EVAL";

    private const string LimitFunction = "\n\nfunc limit(string : String, length : int) -> String:"
        + "\n    if string.length() <= length:"
        + "\n        return string;"
        + "\n    return string.substr(0, length);";

    private const string WrapFunction = "\n\nfunc wordwrap(text : String, max_width : int) -> String:"
        + "\n    var result = \"\";"
        + "\n    var lines = text.split(\"\\n\", false);"
        + "\n    for line in lines:"
        + "\n        var words = line.split(\" \", false);"
        + "\n        var line2 = \"\";"
        + "\n        "
        + "\n        for word in words:"
        + "\n            var candidate = line2 + (\" \" if line2 != \"\" else \"\") + word;"
        + "\n            if candidate.length() > max_width:"
        + "\n                if result != \"\":"
        + "\n                    result += '\\n';"
        + "\n                result += line2;"
        + "\n                line2 = word;"
        + "\n            else:"
        + "\n                line2 = candidate;"
        + "\n        "
        + "\n        if result != \"\":"
        + "\n            result += '\\n';"
        + "\n        result += line2;"
        + "\n    "
        + "\n    return result;";

    /* Public properties. */
    /// <summary>
    /// The current input values for the preview evaluation method.
    /// </summary>
    public PreviewInput Input { get; set; } = new();

    /* Private properties. */
    private GDScript Script { get; set; }
    private GodotObject Instance { get; set; }

    /* Constructors. */
    public Preview(string name) : this(name, "") { }

    public Preview(string name, string code)
    {
        // Generate source.
        if (code == null)
            code = "";
        if (code == "")
            code = "return \"\";";
        code = code.Replace("\t", "    ");

        string source = SkeletonCode.Replace("EVAL", code.Replace("\n", "\n    "));

        // Replace [[key]] statements.
        ReadOnlySpan<char> span = source.AsSpan();
        StringBuilder sb = new(source.Length);
        for (int i = 0; i < span.Length; i++)
        {
            // Find the next key.
            int start = span.Slice(i).IndexOf("[[");
            if (start == -1)
            {
                sb.Append(span.Slice(i));
                break;
            }

            // Append the text before [[.
            sb.Append(span.Slice(i, start));
            i += start + 2;

            // Find end of key.
            int end = span.Slice(i).IndexOf("]]");
            if (end == -1)
            {
                sb.Append("[[").Append(span.Slice(i));
                break;
            }

            // Extract the key and replace it.
            string key = span.Slice(i, end).ToString();
            if (key.StartsWith("element"))
                key = key.Substring(7);
            else
                key = $"\"{key}\"";
            string replacement = $"_input.GetValue({key})";
            sb.Append(replacement);

            i += end + 1;
        }

        code = $"# {name} preview.\n" + sb.ToString();

        // Add functions if they are used.
        if (code.Contains("limit("))
            code += LimitFunction;
        if (code.Contains("wrap("))
            code += WrapFunction;

        // Generate evaluation class.
        GD.Print(code);
        Script = new();
        Script.SourceCode = code;
        Script.Reload();

        // Create instance.
        Instance = (GodotObject)Script.New();
    }

    public Preview(string name, string code, PreviewInput input) : this(name, code)
    {
        Input = input;
    }

    /* Public methods. */
    /// <summary>
    /// Evaluate the preview, using its current input.
    /// </summary>
    public string Evaluate()
    {
        return (string)Instance.Call("eval", Input);
    }
}