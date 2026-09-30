using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A codec representing a language definition.
/// </summary>
internal sealed class LangCodec : Codec
{
    /* Constants. */
    public const string TAG = "lang";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID];

    /* Constructors. */
    public LangCodec() : base() { }
}