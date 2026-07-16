using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class TdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "tdef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["id"];
    protected override HashSet<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, CdefCodec.TAG, TAG, LdefCodec.TAG];

    /* Constructors. */
    public TdefCodec(XmlNode xml) : base(xml) { }
}