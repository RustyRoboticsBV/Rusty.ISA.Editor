using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class OutCodec : Codec
{
    /* Constants. */
    public const string TAG = "out";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Type];

    /* Constructors. */
    public OutCodec() : base() { }
}