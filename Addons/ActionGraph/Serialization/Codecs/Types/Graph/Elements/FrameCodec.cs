using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class FrameCodec : Codec
{
    /* Constants. */
    public const string TAG = "frame";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID, X, Y, Width, Height, Member, Text, Color];

    /* Constructors. */
    public FrameCodec(XmlNode xml) : base(xml) { }
}