using System;
using System.Security.Cryptography;

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
        file.Hash(md5);
        byte[] hashBytes = md5.TransformFinalBlock([], 0, 0);
        string hashHex = Convert.ToHexString(md5.Hash);
        file.SetAttribute(Codec.Checksum, hashHex);

        // Serialize.
        string text = file.Serialize();
        text = InsertComment(text, "Metadata", [MetaCodec.TAG]);
        text = InsertComment(text, "Schema", [IdefCodec.TAG, NdefCodec.TAG]);
        text = InsertComment(text, "Graph", [NodeCodec.TAG, JointCodec.TAG, FrameCodec.TAG, MemoCodec.TAG, EdgeCodec.TAG]);
        return "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"
            + "<!-- Generator: ActionGraph Editor -->\n"
            + text;
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
