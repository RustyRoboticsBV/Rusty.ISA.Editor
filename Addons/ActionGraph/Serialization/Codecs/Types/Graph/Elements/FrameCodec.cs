using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class FrameCodec : Codec
{
    /* Constants. */
    public const string TAG = "frame";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID, X, Y, Width, Height, Member, Text, Color];

    /* Constructors. */
    public FrameCodec(XmlNode xml) : base(xml) { }
}