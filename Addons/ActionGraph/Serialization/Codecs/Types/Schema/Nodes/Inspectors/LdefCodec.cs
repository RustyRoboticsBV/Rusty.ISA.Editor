using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class LdefCodec : InspectorDefinitionCodec, ICodecGroup<InspectorDefinitionCodec>
{
    /* Constants. */
    public const string TAG = "ldef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID];
    protected override List<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, CdefCodec.TAG, TdefCodec.TAG, TAG];

    /* Constructors. */
    public LdefCodec() : base() { }
}