using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class OadefCodec : Codec
{
    /* Constants. */
    public const string TAG = "oadef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID, Type, HideDefault];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public OadefCodec(XmlNode xml) : base(xml) { }
}