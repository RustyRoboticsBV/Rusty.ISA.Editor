using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed partial class OdefCodec : InspectorDefinitionCodec, ICodecGroup<InspectorDefinitionCodec>
{
    /* Constants. */
    public const string TAG = "odef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID];
    protected override List<string> AllowedChildren => [FdefCodec.TAG, TAG, CdefCodec.TAG, TdefCodec.TAG, LdefCodec.TAG];

    /* Constructors. */
    public OdefCodec() : base() { }

    public OdefCodec(XmlNode xml) : base(xml) { }
}