using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class ElemsCodec : Codec
{
    /* Constants. */
    public const string TAG = "elems";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => ["node", "joint", "frame", "memo"];

    /* Constructors. */
    public ElemsCodec(XmlNode xml) : base(xml) { }
}