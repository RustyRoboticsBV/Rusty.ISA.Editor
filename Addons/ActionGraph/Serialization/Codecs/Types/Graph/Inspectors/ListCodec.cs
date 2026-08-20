using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class ListCodec : InspectorCodec, ICodecGroup<InspectorCodec>
{
    /* Constants. */
    public const string TAG = "list";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [Type];
    protected override HashSet<string> AllowedChildren => [FormCodec.TAG, OptionCodec.TAG, ChoiceCodec.TAG, TupleCodec.TAG, TAG];

    /* Constructors. */
    public ListCodec(XmlNode xml) : base(xml) { }
}