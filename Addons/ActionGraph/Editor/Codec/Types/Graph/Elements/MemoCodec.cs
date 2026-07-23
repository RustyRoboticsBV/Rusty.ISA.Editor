using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class MemoCodec : Codec
{
    /* Constants. */
    public const string TAG = "memo";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID, X, Y, Member, Text, Color];

    /* Constructors. */
    public MemoCodec(XmlNode xml) : base(xml) { }
}