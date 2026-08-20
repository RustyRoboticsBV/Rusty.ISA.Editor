using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class OptionCodec : InspectorCodec, ICodecGroup<InspectorCodec>
{
    /* Constants. */
    public const string TAG = "option";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [Type];
    protected override HashSet<string> AllowedChildren => [FormCodec.TAG, TAG, ChoiceCodec.TAG, TupleCodec.TAG, ListCodec.TAG];

    /* Constructors. */
    public OptionCodec(XmlNode xml) : base(xml) { }
}