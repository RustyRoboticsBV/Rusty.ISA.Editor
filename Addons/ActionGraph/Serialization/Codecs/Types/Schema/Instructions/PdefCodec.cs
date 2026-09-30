using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class PdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "pdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Localizable];
    protected override List<string> AllowedChildren => [];

    /* Constructors. */
    public PdefCodec() : base() { }

    public PdefCodec(XmlNode xml) : base(xml) { }
}