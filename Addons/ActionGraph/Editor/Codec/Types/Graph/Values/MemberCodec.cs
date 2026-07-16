using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class MemberCodec : Codec
{
    /* Constants. */
    public const string TAG = "member";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public MemberCodec(XmlNode xml) : base(xml) { }
}