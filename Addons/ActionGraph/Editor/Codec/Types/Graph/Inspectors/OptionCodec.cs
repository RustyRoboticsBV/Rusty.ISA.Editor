using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class OptionCodec : InspectorCodec
{
    /* Constants. */
    public const string TAG = "option";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [FormCodec.TAG, TAG, ChoiceCodec.TAG, TupleCodec.TAG, ListCodec.TAG];

    /* Constructors. */
    public OptionCodec(XmlNode xml) : base(xml) { }
}