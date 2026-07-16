using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class FrameCodec : Codec
{
    /* Constants. */
    public const string TAG = "frame";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [XCodec.TAG, YCodec.TAG, WidthCodec.TAG, HeightCodec.TAG, MemberCodec.TAG, TextCodec.TAG, ColorCodec.TAG];

    /* Constructors. */
    public FrameCodec(XmlNode xml) : base(xml) { }
}