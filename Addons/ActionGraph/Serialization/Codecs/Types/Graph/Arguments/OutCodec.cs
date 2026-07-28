using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class OutCodec : Codec
{
    /* Constants. */
    public const string TAG = "out";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [Type];

    /* Constructors. */
    public OutCodec(XmlNode xml) : base(xml) { }
}