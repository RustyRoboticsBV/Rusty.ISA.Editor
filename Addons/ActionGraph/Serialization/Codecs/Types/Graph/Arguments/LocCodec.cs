using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class LocCodec : Codec
{
    /* Constants. */
    public const string TAG = "loc";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.Type, Codecs.Value];

    /* Constructors. */
    public LocCodec() : base() { }

    public LocCodec(XmlNode xml) : base(xml) { }
}