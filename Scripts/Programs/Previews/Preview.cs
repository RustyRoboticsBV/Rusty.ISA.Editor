using Godot;
using System;
using System.Text;

namespace Rusty.ISA.Editor;

/// <summary>
/// A class that can evaluate an object preview.
/// </summary>
public abstract class Preview
{
    /* Public constants. */
    public const string DisplayName = "name";
    public const string Element = "element";

    /* Fields. */
    private const string SkeletonCode = "func eval(_input) -> String:\n    EVAL";

    private const string LimitFunction = "\n\nfunc limit(string : String, length : int, truncate_suffix : String) -> String:"
        + "\n    if string.length() <= length:"
        + "\n        return string;"
        + "\n    return string.substr(0, length) + truncate_suffix;";

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
    public PreviewInput DefaultInput { get; private set; } = new();

    /* Private properties. */
    private GDScript Script { get; set; }
    private bool IsReloaded { get; set; }

    /* Constructors. */
    public Preview(string code)
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
            if (key != Element && key.StartsWith(Element))
            {
                string elementIndex = key.Substring(Element.Length);
                if (int.TryParse(elementIndex, out int index))
                    key = $"\"{key}\"";
                else
                    key = $"\"{Element}\" + str({elementIndex})";
            }
            else
                key = $"\"{key}\"";
            string replacement = $"_input.GetValue({key})";
            sb.Append(replacement);

            i += end + 1;
        }
        source = sb.ToString();

        // Add functions if they are used.
        if (source.Contains("limit("))
            source += LimitFunction;
        if (source.Contains("wrap("))
            source += WrapFunction;

        // Generate evaluation class.
        // We don't reload the script yet to avoid unnecessary overhead for unused previews.
        Script = new();
        Script.SourceCode = source;
    }

    /* Public methods. */
    public override string ToString()
    {
        return Script.SourceCode;
    }

    /// <summary>
    /// Create an instance of this preview.
    /// </summary>
    public abstract PreviewInstance CreateInstance();

    /* Internal methods. */
    /// <summary>
    /// Emit an instance of the generated preview script.
    /// </summary>
    internal GodotObject Emit()
    {
        // Reload the class if it wasn't done yet.
        if (!IsReloaded)
        {
            Error error = Script.Reload();
            IsReloaded = true;
        }

        // Create new instance of script.
        return Script.New().AsGodotObject();
    }
}