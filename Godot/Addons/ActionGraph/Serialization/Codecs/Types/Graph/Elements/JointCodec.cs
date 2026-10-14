using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class JointCodec : Codec
{
    /* Constants. */
    public const string TAG = "joint";
    public override string Name => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [
        Codecs.ID,
        Codecs.X, Codecs.Y, Codecs.ParentFrame, Codecs.Edge
    ];

    /* Constructors. */
    public JointCodec() : base() { }
}