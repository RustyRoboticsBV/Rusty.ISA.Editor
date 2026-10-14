using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class ArgCodec : Codec
{
    /* Constants. */
    public const string TAG = "arg";
    public override string Name => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Definition, Codecs.Value];
    protected override List<string> AllowedChildren => [LocCodec.TAG];

    /* Constructors. */
    public ArgCodec() : base() { }
}