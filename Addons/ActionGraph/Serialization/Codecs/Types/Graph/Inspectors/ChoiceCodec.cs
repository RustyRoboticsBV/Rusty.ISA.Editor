using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class ChoiceCodec : InspectorCodec, ICodecGroup<InspectorCodec>
{
    /* Constants. */
    public const string TAG = "choice";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.Type];
    protected override List<string> AllowedChildren => [FormCodec.TAG, OptionCodec.TAG, TAG, TupleCodec.TAG, ListCodec.TAG];

    /* Constructors. */
    public ChoiceCodec() : base() { }

    public ChoiceCodec(XmlNode xml) : base(xml) { }
}