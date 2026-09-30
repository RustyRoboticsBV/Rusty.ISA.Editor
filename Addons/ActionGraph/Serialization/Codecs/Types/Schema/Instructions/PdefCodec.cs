using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class PdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "pdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Localizable];

    /* Constructors. */
    public PdefCodec() : base() { }
}