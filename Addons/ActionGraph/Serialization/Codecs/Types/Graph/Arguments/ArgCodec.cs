using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class ArgCodec : Codec
{
    /* Constants. */
    public const string TAG = "arg";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.Type, Codecs.Value];
    protected override List<string> AllowedChildren => [LocCodec.TAG];

    /* Constructors. */
    public ArgCodec() : base() { }

    public ArgCodec(XmlNode xml) : base(xml) { }
}