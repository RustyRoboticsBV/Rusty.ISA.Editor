using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class NodeCodec : Codec
{
    /* Constants. */
    public const string TAG = "node";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID, Type, X, Y, Member, Start];
    protected override HashSet<string> AllowedChildren => [
        FormCodec.TAG, OptionCodec.TAG, ChoiceCodec.TAG, TupleCodec.TAG, ListCodec.TAG
    ];

    /* Constructors. */
    public NodeCodec(XmlNode xml) : base(xml) { }
}