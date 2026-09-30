using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class EdgeCodec : Codec
{
    /* Constants. */
    public const string TAG = "edge";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [
        Codecs.ID,
        Codecs.From, Codecs.Port, Codecs.To
    ];

    /* Constructors. */
    public EdgeCodec() : base() { }
}