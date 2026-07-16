using Godot;
using System;
using System.Security.Cryptography;
using System.Xml;

using Rusty.ActionGraph.Runtime;
using Rusty.ActionGraph.Compilation;

namespace Rusty.ActionGraph.Serialization;

[GlobalClass]
public sealed partial class XmlLoader : Node
{
    /// <summary>
    /// Serialize an ActionGraph node tree into a string of XML.
    /// </summary>
    public static string Serialize(FileCodec file)
    {
        // Compute checksum.
        MetaCodec meta = file.GetFirstChild<MetaCodec>();
        if (meta == null)
        {
            meta = new();
            file.AddChild(meta);
        }

        CheckCodec check = meta.GetFirstChild<CheckCodec>();
        if (check == null)
        {
            check = new();
            meta.AddChild(check);
        }

        MD5 md5 = MD5.Create();
        file.Hash(md5);
        byte[] hashBytes = md5.TransformFinalBlock([], 0, 0);
        string hashHex = Convert.ToHexString(md5.Hash);
        check.InnerText = hashHex;

        // Serialize.
        return file.Serialize();
    }

    /// <summary>
    /// Load a string of XML as an ActionGraph node tree.
    /// </summary>
    public static FileCodec Load(string xml)
    {
        // Load XML.
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        // Parse DOM.
        foreach (XmlNode node in doc)
        {
            if (node is XmlElement)
            {
                Codec codec = Codec.Load(node);
                if (codec is FileCodec file)
                    return file;
                else
                    throw new InvalidCastException($"Files must have a <{FileCodec.TAG}> root element.");
            }
        }
        throw new FormatException("Empty XML file!");
    }

    /// <summary>
    /// Load a string of XML as a InstructionProgram resource.
    /// </summary>
    public static InstructionProgram LoadAsProgram(string xml)
    {
        FileCodec file = Load(xml);
        return Compiler.Compile(file);
    }
}
