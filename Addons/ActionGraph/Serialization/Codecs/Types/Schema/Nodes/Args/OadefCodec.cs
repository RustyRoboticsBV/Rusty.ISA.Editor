using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class OadefCodec : Codec
{
    /* Constants. */
    public const string TAG = "oadef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID, Type, NoDefault];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public OadefCodec(XmlNode xml) : base(xml) { }
}