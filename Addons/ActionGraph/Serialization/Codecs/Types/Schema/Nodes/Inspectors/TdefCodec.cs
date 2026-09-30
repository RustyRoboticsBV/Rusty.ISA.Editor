using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class TdefCodec : InspectorDefinitionCodec, ICodecGroup<InspectorDefinitionCodec>
{
    /* Constants. */
    public const string TAG = "tdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID];
    protected override List<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, CdefCodec.TAG, TAG, LdefCodec.TAG];

    /* Constructors. */
    public TdefCodec() : base() { }
}