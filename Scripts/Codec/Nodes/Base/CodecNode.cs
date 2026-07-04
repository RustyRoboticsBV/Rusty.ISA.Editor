using System;
using System.Security.Cryptography;
using System.Text;

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
}