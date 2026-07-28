using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for serializing FileCodec objects to XML.
/// </summary>
public static class Serializer
{
    /// <summary>
    /// Serialize a FileCodec to a string of XML.
    /// </summary>
    public static string Serialize(FileCodec file)
    {
        // Compute checksum.
        MD5 md5 = MD5.Create();
        Hash(file, md5);
        byte[] hashBytes = md5.TransformFinalBlock([], 0, 0);
        string hashHex = Convert.ToHexString(md5.Hash);
        file.SetAttribute(Codec.Checksum, hashHex);

        // Serialize.
        string text = SerializeCodec(file);
        text = InsertComment(text, "Metadata", [MetaCodec.TAG]);
        text = InsertComment(text, "Schema", [IdefCodec.TAG, NdefCodec.TAG]);
        text = InsertComment(text, "Graph", [NodeCodec.TAG, JointCodec.TAG, FrameCodec.TAG, MemoCodec.TAG, EdgeCodec.TAG]);
        return "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"
            + "<!-- Generator: ActionGraph Editor -->\n\n"
            + text;
    }

    /* Private methods. */
    /// <summary>
    /// Compute the checksum of this node and its child nodes.
    /// </summary>
    private static void Hash(Codec codec, HashAlgorithm hash)
    {
        // Hash start tag.
        Hash(hash, "<");
        Hash(hash, codec.GetTag());

        foreach (var attribute in codec.Attributes)
        {
            if (attribute.Key == Codec.Checksum)
                continue;

            Hash(hash, " ");
            Hash(hash, attribute.Key);
            Hash(hash, "=\"");
            Hash(hash, attribute.Value);
            Hash(hash, "\"");
        }

        Hash(hash, ">");

        // Hash contents.
        if (codec.Children.Count == 0)
            Hash(hash, codec.InnerText);
        else
        {
            foreach (Codec child in codec.Children)
            {
                Hash(child, hash);
            }
        }

        // Hash end tag.
        Hash(hash, "</");
        Hash(hash, codec.GetTag());
        Hash(hash, ">");
    }

    /// <summary>
    /// Compute the checksum of a string.
    /// </summary>
    private static void Hash(HashAlgorithm hash, string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        hash.TransformBlock(bytes, 0, bytes.Length, null, 0);
    }

    /// <summary>
    /// Convert this node to XML.
    /// </summary>
    private static string SerializeCodec(Codec codec)
    {
        // Handle attributes.
        StringBuilder attributes = new();
        foreach (var attr in codec.Attributes)
        {
            if (!codec.AllowsAttribute(attr.Key))
                throw new KeyNotFoundException($"Codec '{codec.GetType().Name}' does not allow name {attr.Key}.");

            attributes.Append(' ');
            attributes.Append(attr.Key);
            attributes.Append("=\"");
            attributes.Append(attr.Value);
            attributes.Append('"');
        }

        // Handle children.
        StringBuilder children = new();
        foreach (Codec child in codec.Children)
        {
            if (!codec.AllowsChild(child.GetTag()))
                throw new KeyNotFoundException($"Codec '{codec.GetType().Name}' does not allow child elements with xml tag '{child.GetTag()}'.");

            if (children.Length > 0)
                children.Append('\n');
            children.Append(SerializeCodec(child));
        }

        // Build XML.
        StringBuilder xml = new();
        xml.Append('<');
        xml.Append(codec.GetTag());
        xml.Append(attributes.ToString());

        if (children.Length > 0)
        {
            xml.Append(">");

            xml.Append("\n\t");
            xml.Append(children.ToString().Replace("\n", "\n\t"));

            xml.Append("\n</");
            xml.Append(codec.GetTag());
            xml.Append(">");
        }
        else if (codec.InnerText.Length > 0)
        {
            xml.Append(">");

            xml.Append(codec.InnerText);

            xml.Append("</");
            xml.Append(codec.GetTag());
            xml.Append(">");
        }
        else if (children.Length == 0 && codec.InnerText.Length == 0)
            xml.Append("/>");

        return xml.ToString();
    }

    /// <summary>
    /// Insert an XML comment before a block of XM elements and return the result.
    /// </summary>
    private static string InsertComment(string text, string comment, string[] tags)
    {
        int index = -1;
        foreach (string tag in tags)
        {
            int index2 = text.IndexOf($"<{tag}");
            if (index == -1 || index2 < index)
                index = index2;
        }
        if (index >= 0)
            return text.Insert(index, $"\n\t<!-- {comment} -->\n\t");
        else
            return text;
    }
}
