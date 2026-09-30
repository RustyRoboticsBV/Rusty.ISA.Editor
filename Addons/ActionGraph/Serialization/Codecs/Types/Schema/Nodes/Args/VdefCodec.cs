using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class VdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "vdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Type];

    /* Constructors. */
    public VdefCodec() : base() { }
}