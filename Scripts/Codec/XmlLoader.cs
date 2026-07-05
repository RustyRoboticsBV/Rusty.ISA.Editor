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

    public static VirtualProgram LoadAsProgram(string str)
    {
        FileNode file = Load(str);

        Dictionary<string, string> metadata = new();
        foreach (var field in file.Meta.Fields)
        {
            metadata.Add(field.ID, field.Value);
        }

        VirtualProgram program = new(metadata, file.ToInstructions().ToArray());

        if (metadata.TryGetValue("title", out string title))
            program.ResourceName = title;
        else if (metadata.TryGetValue("name", out string name))
            program.ResourceName = name;

        return program;
    }
}
