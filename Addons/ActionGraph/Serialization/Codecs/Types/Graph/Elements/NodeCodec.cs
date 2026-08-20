using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class NodeCodec : Codec, ICodecGroup<InspectorCodec>
{
    /* Constants. */
    public const string TAG = "node";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID, Type, X, Y, Member, Start];
    protected override HashSet<string> AllowedChildren => [
        FormCodec.TAG, OptionCodec.TAG, ChoiceCodec.TAG, TupleCodec.TAG, ListCodec.TAG
    ];

    /* Constructors. */
    public NodeCodec(XmlNode xml) : base(xml) { }
}