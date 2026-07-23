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
        MD5 md5 = MD5.Create();
        file.Hash(md5);
        byte[] hashBytes = md5.TransformFinalBlock([], 0, 0);
        string hashHex = Convert.ToHexString(md5.Hash);
        file.SetAttribute(Codec.Checksum, hashHex);

        // Serialize.
        string text = file.Serialize();
        text = InsertComment(text, "Metadata", [MetaCodec.TAG]);
        text = InsertComment(text, "Schema", [IdefCodec.TAG, NdefCodec.TAG]);
        text = InsertComment(text, "Graph", [NodeCodec.TAG, JointCodec.TAG, FrameCodec.TAG, MemoCodec.TAG, EdgeCodec.TAG]);
        return "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<!-- Generator: ActionGraph Editor -->\n" + text;
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

    /* Private methods. */
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
