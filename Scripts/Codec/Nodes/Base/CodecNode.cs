using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A codec node.
/// </summary>
public abstract class CodecNode
{
    /* Public methods. */
    /// <summary>
    /// Convert this node to XML.
    /// </summary>
    public abstract string Serialize();

    /// <summary>
    /// Compute the checksum of this node and its child nodes.
    /// </summary>
    public abstract void Hash(HashAlgorithm hash);

    /* Protected methods. */
    /// <summary>
    /// Wrap a string of XML within a tag.
    /// </summary>
    protected static string Wrap(string innerXml, string tag) => Wrap(innerXml, tag, null);

    /// <summary>
    /// Wrap a string of XML within a tag, with an optional id attribute.
    /// </summary>
    protected static string Wrap(string innerXml, string tag, string id = null)
    {
        if (string.IsNullOrWhiteSpace(tag))
            throw new ArgumentException("may not be emty or whitespaces.", nameof(tag));

        const string TAB = "\t";

        string attributeId = "";
        if (id != null)
            attributeId = $" id=\"{id}\"";

        if (string.IsNullOrEmpty(innerXml))
            return $"<{tag}{attributeId}/>";
        else if (innerXml.StartsWith('<'))
            return $"<{tag}{attributeId}>\n" + TAB + innerXml.Replace("\n", $"\n{TAB}") + $"\n</{tag}>";
        else
            return $"<{tag}{attributeId}>{innerXml}</{tag}>";
    }

    /// <summary>
    /// Compute the checksum of a string.
    /// </summary>
    protected static void Hash(HashAlgorithm hash, string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        hash.TransformBlock(bytes, 0, bytes.Length, null, 0);
    }

    /// <summary>
    /// Compute the checksum of a start tag.
    /// </summary>
    protected static void StartHash(HashAlgorithm hash, string tag, string id = null)
    {
        string idStr = null;
        if (id != null)
            idStr = $" id=\"{id}\"";

        Hash(hash, $"<{tag}{idStr}>");
    }

    /// <summary>
    /// Compute the checksum of an end tag.
    /// </summary>
    protected static void EndHash(HashAlgorithm hash, string tag) => Hash(hash, $"</{tag}>");

    /// <summary>
    /// Compute the checksum of a start and end tag.
    /// </summary>
    protected static void EmptyHash(HashAlgorithm hash, string tag, string id = null)
    {
        string idStr = null;
        if (id != null)
            idStr = $" id=\"{id}\"";

        Hash(hash, $"<{tag}{idStr}/>");
    }

    /// <summary>
    /// Append a line to a string builder.
    /// </summary>
    protected static void AppendLine(StringBuilder sb, string text)
    {
        if (sb.Length == 0)
            sb.Append(text);
        else
        {
            sb.Append('\n');
            sb.Append(text);
        }
    }

    /// <summary>
    /// Get the first attribute of an XML node called "id". Returns null if here wasn't one.
    /// </summary>
    protected static string GetId(XmlNode node)
    {
        foreach (XmlAttribute attribute in node.Attributes)
        {
            if (attribute.Name == "id")
                return attribute.Value;
        }
        return null;
    }

    /// <summary>
    /// Make sure that an XML's tag matches a value. Otherwise, throw an exception.
    /// </summary>
    protected static void CheckTagMismatch(XmlNode xml, string tag)
    {
        if (xml.Name != tag)
            throw new FormatException($"Encountered '{xml.Name}', expected '{tag}'!");
    }

    /// <summary>
    /// Throw an invalid child XML node exception.
    /// </summary>
    protected static FormatException InvalidChildException(XmlNode xml, string parentTag)
    {
        return new FormatException($"Encountered '{xml.Name}' as a child of '{parentTag}'!");
    }
}