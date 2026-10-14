using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class NodeCodec : Codec, ICodecGroup<InspectorCodec>
{
    /* Constants. */
    public const string TAG = "node";
    public override string Name => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [
        Codecs.ID, Codecs.Definition,
        Codecs.X, Codecs.Y, Codecs.Width, Codecs.Height, Codecs.Folded,
        Codecs.ParentFrame, Codecs.Start
    ];
    protected override List<string> AllowedChildren => [
        FormCodec.TAG, OptionCodec.TAG, ChoiceCodec.TAG, TupleCodec.TAG, ListCodec.TAG
    ];

    /* Constructors. */
    public NodeCodec() : base() { }
}