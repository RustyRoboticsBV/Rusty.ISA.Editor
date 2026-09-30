using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class OutCodec : Codec
{
    /* Constants. */
    public const string TAG = "out";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.Type];

    /* Constructors. */
    public OutCodec() : base() { }

    public OutCodec(XmlNode xml) : base(xml) { }
}