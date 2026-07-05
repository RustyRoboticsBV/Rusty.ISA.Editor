using Godot;
using Godot.Collections;
using System;
using System.Security.Cryptography;
using System.Xml;

using Rusty.ActionGraph.Runtime;

namespace Rusty.ActionGraph.Serialization;

[GlobalClass]
public sealed partial class XmlLoader : Node
{
    /// <summary>
    /// Serialize an ActionGraph node tree into a string of XML.
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
    /// Load a string of XML as an ActionGraph node tree.
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
        throw new FormatException("Empty XML file!");
    }

    /// <summary>
    /// Load a string of XML as a InstructionProgram resource.
    /// </summary>
    public static InstructionProgram LoadAsProgram(string xml)
    {
        FileNode file = Load(xml);
        return file.ToProgram();
    }
}
