using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class MemoCodec : Codec
{
    /* Constants. */
    public const string TAG = "memo";
    public override string Name => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [
        Codecs.ID,
        Codecs.X, Codecs.Y, Codecs.Width, Codecs.Height, Codecs.Folded,
        Codecs.ParentFrame,
        Codecs.Text, Codecs.Color
    ];

    /* Constructors. */
    public MemoCodec() : base() { }
}