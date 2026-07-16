using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class NodeCodec : Codec
{
    /* Constants. */
    public const string TAG = "node";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["id", "type"];
    protected override HashSet<string> AllowedChildren => [
        XCodec.TAG, YCodec.TAG, MemberCodec.TAG, StartCodec.TAG,
        FormCodec.TAG, OptionCodec.TAG, ChoiceCodec.TAG, TupleCodec.TAG, ListCodec.TAG
    ];

    /* Constructors. */
    public NodeCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public static void Register() => Codecs.Add(TAG, typeof(NodeCodec));
}