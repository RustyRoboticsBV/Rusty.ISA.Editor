using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class FrameCodec : Codec
{
    /* Constants. */
    public const string TAG = "frame";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [
        Codecs.ID,
        Codecs.X, Codecs.Y, Codecs.Width, Codecs.Height,
        Codecs.Member, Codecs.Text, Codecs.Color
    ];

    /* Constructors. */
    public FrameCodec() : base() { }

    public FrameCodec(XmlNode xml) : base(xml) { }
}