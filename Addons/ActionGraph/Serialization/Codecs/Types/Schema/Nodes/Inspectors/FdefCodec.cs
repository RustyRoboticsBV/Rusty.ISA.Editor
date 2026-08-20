using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class FdefCodec : InspectorDefinitionCodec
{
    /* Constants. */
    public const string TAG = "fdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID, Type];
    protected override HashSet<string> AllowedChildren => [VadefCodec.TAG, OadefCodec.TAG];

    /* Constructors. */
    public FdefCodec(XmlNode xml) : base(xml) { }
}