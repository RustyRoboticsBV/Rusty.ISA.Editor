using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class FileCodec : Codec
{
    /* Constants. */
    public const string TAG = "file";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedChildren => [
        MetaCodec.TAG, LangCodec.TAG,
        IdefCodec.TAG, NdefCodec.TAG,
        NodeCodec.TAG, JointCodec.TAG, FrameCodec.TAG, MemoCodec.TAG, EdgeCodec.TAG
    ];
    protected override HashSet<string> AllowedAttributes => [Checksum];

    /* Constructors. */
    public FileCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    /// <summary>
    /// Find an IdefCodec with some ID. Returns null if it doesn't exist.
    /// </summary>
    public IdefCodec FindIdef(string id)
    {
        foreach (Codec child in Children)
        {
            if (child is IdefCodec idef && idef.GetAttribute(ID) == id)
                return idef;
        }
        return null;
    }

    /// <summary>
    /// Find an NdefCodec with some ID. Returns null if it doesn't exist.
    /// </summary>
    public NdefCodec FindNdef(string id)
    {
        foreach (Codec child in Children)
        {
            if (child is NdefCodec ndef && ndef.GetAttribute(ID) == id)
                return ndef;
        }
        return null;
    }
}