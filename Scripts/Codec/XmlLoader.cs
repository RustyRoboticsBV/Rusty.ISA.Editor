using System;
using System.Security.Cryptography;
using System.Xml;

namespace Rusty.ISA.Serialization;

public static class XmlLoader
{
    public static string Serialize(FileNode node)
    {
        // Compute checksum.
        if (node.Meta == null)
            node.Meta = new(null, null);
        if (node.Meta.Checksum == null)
            node.Meta.Checksum = new("");

        MD5 md5 = MD5.Create();
        node.Hash(md5);
        byte[] hashBytes = md5.TransformFinalBlock([], 0, 0);
        string hashHex = Convert.ToHexString(hashBytes);
        node.Meta.Checksum.String = hashHex;

        // Serialize.
        return node.Serialize();
    }

    public static FileNode Load(string str)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(str);
        foreach (XmlNode node in doc)
        {
            if (node is XmlElement)
                return FileNode.Load(node);
        }
        throw new FormatException("Empty ISA file!");
    }
}
