using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class ChoiceCodec : InspectorCodec
{
    /* Constants. */
    public const string TAG = "choice";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["select"];
    protected override HashSet<string> AllowedChildren => [FormCodec.TAG, OptionCodec.TAG, TAG, TupleCodec.TAG, ListCodec.TAG];

    /* Constructors. */
    public ChoiceCodec(XmlNode xml) : base(xml) { }
}