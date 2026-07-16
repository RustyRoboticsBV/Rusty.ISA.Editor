using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class MemoCodec : Codec
{
    /* Constants. */
    public const string TAG = "memo";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [XCodec.TAG, YCodec.TAG, MemberCodec.TAG, TextCodec.TAG, ColorCodec.TAG];

    /* Constructors. */
    public MemoCodec(XmlNode xml) : base(xml) { }
}