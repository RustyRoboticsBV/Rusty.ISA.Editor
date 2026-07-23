using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class ListCodec : InspectorCodec
{
    /* Constants. */
    public const string TAG = "list";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [Type];
    protected override HashSet<string> AllowedChildren => [FormCodec.TAG, OptionCodec.TAG, ChoiceCodec.TAG, TupleCodec.TAG, TAG];

    /* Constructors. */
    public ListCodec(XmlNode xml) : base(xml) { }
}