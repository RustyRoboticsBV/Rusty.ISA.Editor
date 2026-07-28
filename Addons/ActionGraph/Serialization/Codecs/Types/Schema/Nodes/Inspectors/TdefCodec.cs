using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class TdefCodec : CollectionDefinitionCodec
{
    /* Constants. */
    public const string TAG = "tdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, CdefCodec.TAG, TAG, LdefCodec.TAG];

    /* Constructors. */
    public TdefCodec(XmlNode xml) : base(xml) { }
}