using System.Text;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A serializer codec.
/// </summary>
internal static class CodecPrinter
{
    /* Public methods. */
    /// <summary>
    /// Return the string representation of a codec.
    /// </summary>
    public static string ToString(Codec codec)
    {
        StringBuilder sb = new();
        AppendToString(codec, sb, "", true, true, true);
        return sb.ToString();
    }

    /// <summary>
    /// Return the string representation of a codec.
    /// </summary>
    public static string ToString(Codec codec, bool omitChildren)
    {
        if (!omitChildren)
            return ToString(codec);

        StringBuilder sb = new();
        AppendToString(codec, sb, "", true, true, false);
        return sb.ToString();
    }

    /* Private methods. */
    /// <summary>
    /// Helper function for ToString.
    /// </summary>
    private static void AppendToString(Codec codec, StringBuilder sb, string prefix, bool last, bool root, bool recurse)
    {
        if (!root)
        {
            sb.Append(prefix);
            sb.Append(last ? "\u2514\u2500" : "\u251C\u2500");
        }

        sb.Append(codec.Tag);

        if (codec.Attributes.Count > 0)
        {
            sb.Append(" {");

            bool first = true;
            foreach (var attribute in codec.Attributes)
            {
                if (!first)
                    sb.Append(", ");

                sb.Append(attribute.Key);
                sb.Append("=\"");
                sb.Append(attribute.Value);
                sb.Append('"');

                first = false;
            }

            sb.Append('}');
        }

        if (recurse)
        {
            sb.AppendLine();

            string childPrefix = root ? "" : prefix + (last ? "  " : "\u2502 ");

            for (int i = 0; i < codec.Children.Count; i++)
            {
                AppendToString(codec.Children[i], sb, childPrefix, i == codec.Children.Count - 1, false, recurse);
            }
        }
        else if (codec.Children.Count > 0)
            sb.Append("...");
    }
}