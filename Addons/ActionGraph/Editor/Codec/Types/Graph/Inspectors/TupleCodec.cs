using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class TupleCodec : Codec
{
    /* Constants. */
    public const string TAG = "tuple";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [FormCodec.TAG, OptionCodec.TAG, ChoiceCodec.TAG, TAG, ListCodec.TAG];

    /* Constructors. */
    public TupleCodec(XmlNode xml) : base(xml) { }
}