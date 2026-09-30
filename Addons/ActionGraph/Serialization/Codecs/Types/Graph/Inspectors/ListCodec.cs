using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class ListCodec : InspectorCodec, ICodecGroup<InspectorCodec>
{
    /* Constants. */
    public const string TAG = "list";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.Type];
    protected override List<string> AllowedChildren => [FormCodec.TAG, OptionCodec.TAG, ChoiceCodec.TAG, TupleCodec.TAG, TAG];

    /* Constructors. */
    public ListCodec() : base() { }
}