using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class FileCodec : Codec
{
    /* Constants. */
    public const string TAG = "file";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Editor, Codecs.Checksum];
    protected override List<string> AllowedChildren => [
        MetaCodec.TAG, LangCodec.TAG,
        IdefCodec.TAG, NdefCodec.TAG,
        NodeCodec.TAG, JointCodec.TAG, FrameCodec.TAG, MemoCodec.TAG, EdgeCodec.TAG
    ];

    /* Constructors. */
    public FileCodec() : base() { }

    /* Public methods. */
    /// <summary>
    /// Find an IdefCodec with some ID. Returns null if it doesn't exist.
    /// </summary>
    public IdefCodec FindIdef(string id)
    {
        foreach (Codec child in Children)
        {
            if (child is IdefCodec idef && idef.GetAttribute(Codecs.ID) == id)
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
            if (child is NdefCodec ndef && ndef.GetAttribute(Codecs.ID) == id)
                return ndef;
        }
        return null;
    }
}