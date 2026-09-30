using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class JointCodec : Codec
{
    /* Constants. */
    public const string TAG = "joint";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.X, Codecs.Y, Codecs.Member, Codecs.Edge];

    /* Constructors. */
    public JointCodec() : base() { }

    public JointCodec(XmlNode xml) : base(xml) { }
}