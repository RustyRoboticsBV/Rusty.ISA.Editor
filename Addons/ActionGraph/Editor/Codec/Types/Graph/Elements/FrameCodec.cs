using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class FrameCodec : Codec
{
    /* Constants. */
    public const string TAG = "frame";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["id"];
    protected override HashSet<string> AllowedChildren => ["x", "y", "width", "height", "member", "text", "color"];

    /* Constructors. */
    public FrameCodec(XmlNode xml) : base(xml) { }
}