using Godot;
using Godot.Collections;
using System;
using System.Security.Cryptography;
using System.Xml;

using Rusty.ISA.Runtime;

namespace Rusty.ISA.Serialization;

[GlobalClass]
public sealed partial class XmlLoader : Node
{
    /// <summary>
    /// Serialize a DOM node tree into a string of XML.
    /// </summary>
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

    /// <summary>
    /// Load a string of XML as an ISA node tree.
    /// </summary>
    public static FileNode Load(string xml)
    {
        // Load XML.
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        // Parse DOM.
        foreach (XmlNode node in doc)
        {
            if (node is XmlElement)
                return FileNode.Load(node);
        }
        throw new FormatException("Empty ISA file!");
    }

    /// <summary>
    /// Load a string of XML as an ISA program.
    /// </summary>
    public static VirtualProgram LoadAsProgram(string xml)
    {
        FileNode file = Load(xml);
        return file.ToProgram();
    }
}
