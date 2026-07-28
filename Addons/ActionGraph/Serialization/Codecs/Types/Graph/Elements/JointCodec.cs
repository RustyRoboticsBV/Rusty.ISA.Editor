using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class JointCodec : Codec
{
    /* Constants. */
    public const string TAG = "joint";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID, X, Y, Member];

    /* Constructors. */
    public JointCodec(XmlNode xml) : base(xml) { }
}