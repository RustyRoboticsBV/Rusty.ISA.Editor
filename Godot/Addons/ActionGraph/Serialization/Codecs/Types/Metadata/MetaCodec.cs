using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A codec representing a metadata record.
/// </summary>
internal sealed class MetaCodec : Codec
{
    /* Constants. */
    public const string TAG = "meta";
    public override string Name => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Value];

    /* Constructors. */
    public MetaCodec() : base() { }
}