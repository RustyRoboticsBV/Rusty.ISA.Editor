using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class FdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "fdef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID, Type];
    protected override HashSet<string> AllowedChildren => [VadefCodec.TAG, OadefCodec.TAG];

    /* Constructors. */
    public FdefCodec(XmlNode xml) : base(xml) { }
}