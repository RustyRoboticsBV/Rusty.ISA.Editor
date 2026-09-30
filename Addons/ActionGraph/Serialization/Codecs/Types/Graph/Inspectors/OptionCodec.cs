using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class OptionCodec : InspectorCodec, ICodecGroup<InspectorCodec>
{
    /* Constants. */
    public const string TAG = "option";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.Type];
    protected override List<string> AllowedChildren => [FormCodec.TAG, TAG, ChoiceCodec.TAG, TupleCodec.TAG, ListCodec.TAG];

    /* Constructors. */
    public OptionCodec() : base() { }
}