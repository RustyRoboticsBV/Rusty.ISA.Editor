using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class NdefCodec : Codec, ICodecGroup<InspectorDefinitionCodec>
{
    /* Constants. */
    public const string TAG = "ndef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID];
    protected override List<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, CdefCodec.TAG, TdefCodec.TAG, LdefCodec.TAG];

    /* Constructors. */
    public NdefCodec() : base() { }
}