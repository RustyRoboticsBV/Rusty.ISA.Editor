using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class MetaCodec : Codec
{
    /* Constants. */
    public const string TAG = "meta";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [DataCodec.TAG, CheckCodec.TAG];

    /* Constructors. */
    public MetaCodec() : base() { }

    public MetaCodec(XmlNode xml) : base(xml) { }
}