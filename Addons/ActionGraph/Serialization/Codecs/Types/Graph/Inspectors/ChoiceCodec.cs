using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class ChoiceCodec : InspectorCodec, ICodecGroup<InspectorCodec>
{
    /* Constants. */
    public const string TAG = "choice";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [Type];
    protected override HashSet<string> AllowedChildren => [FormCodec.TAG, OptionCodec.TAG, TAG, TupleCodec.TAG, ListCodec.TAG];

    /* Constructors. */
    public ChoiceCodec(XmlNode xml) : base(xml) { }
}