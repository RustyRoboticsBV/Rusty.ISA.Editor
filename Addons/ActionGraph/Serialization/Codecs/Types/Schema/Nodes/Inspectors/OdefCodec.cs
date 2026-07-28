using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class OdefCodec : InspectorDefinitionCodec, ICodecGroup<InspectorDefinitionCodec>
{
    /* Constants. */
    public const string TAG = "odef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [FdefCodec.TAG, TAG, CdefCodec.TAG, TdefCodec.TAG, LdefCodec.TAG];

    /* Constructors. */
    public OdefCodec(XmlNode xml) : base(xml) { }
}