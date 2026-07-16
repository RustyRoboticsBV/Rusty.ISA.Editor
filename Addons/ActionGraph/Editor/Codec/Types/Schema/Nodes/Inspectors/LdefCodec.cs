using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class LdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "ldef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, CdefCodec.TAG, TdefCodec.TAG, TAG];

    /* Constructors. */
    public LdefCodec(XmlNode xml) : base(xml) { }
}