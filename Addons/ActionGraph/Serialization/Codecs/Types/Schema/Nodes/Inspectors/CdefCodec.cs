using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class CdefCodec : InspectorDefinitionCodec, ICodecGroup<InspectorDefinitionCodec>
{
    /* Constants. */
    public const string TAG = "cdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID];
    protected override List<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, TAG, TdefCodec.TAG, LdefCodec.TAG];

    /* Constructors. */
    public CdefCodec() : base() { }

    public CdefCodec(XmlNode xml) : base(xml) { }
}