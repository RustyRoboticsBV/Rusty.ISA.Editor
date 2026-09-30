using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class FdefCodec : InspectorDefinitionCodec
{
    /* Constants. */
    public const string TAG = "fdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Type];
    protected override List<string> AllowedChildren => [VdefCodec.TAG, JdefCodec.TAG];

    /* Constructors. */
    public FdefCodec() : base() { }

    public FdefCodec(XmlNode xml) : base(xml) { }
}