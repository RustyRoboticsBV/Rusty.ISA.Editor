using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class JdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "jdef";
    public override string Name => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Definition, Codecs.NoDefault];

    /* Constructors. */
    public JdefCodec() : base() { }
}