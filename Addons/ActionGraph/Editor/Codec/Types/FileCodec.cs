using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class FileCodec : Codec
{
    /* Constants. */
    public const string TAG = "file";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedChildren => [
        MetaCodec.TAG,
        IdefCodec.TAG, NdefCodec.TAG,
        NodeCodec.TAG, JointCodec.TAG, FrameCodec.TAG, MemoCodec.TAG, EdgeCodec.TAG
    ];
    protected override HashSet<string> AllowedAttributes => [Checksum];

    /* Constructors. */
    public FileCodec(XmlNode xml) : base(xml) { }
}