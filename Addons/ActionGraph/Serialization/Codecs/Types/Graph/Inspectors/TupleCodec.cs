using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class TupleCodec : InspectorCodec, ICodecGroup<InspectorCodec>
{
    /* Constants. */
    public const string TAG = "tuple";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.Type];
    protected override List<string> AllowedChildren => [FormCodec.TAG, OptionCodec.TAG, ChoiceCodec.TAG, TAG, ListCodec.TAG];

    /* Constructors. */
    public TupleCodec() : base() { }
}