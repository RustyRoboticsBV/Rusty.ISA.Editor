using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class VdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "vdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Type];
    protected override List<string> AllowedChildren => [];

    /* Constructors. */
    public VdefCodec() : base() { }

    public VdefCodec(XmlNode xml) : base(xml) { }
}