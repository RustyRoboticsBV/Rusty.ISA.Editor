using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class JdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "jdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Type, Codecs.NoDefault];
    protected override List<string> AllowedChildren => [];

    /* Constructors. */
    public JdefCodec() : base() { }

    public JdefCodec(XmlNode xml) : base(xml) { }
}