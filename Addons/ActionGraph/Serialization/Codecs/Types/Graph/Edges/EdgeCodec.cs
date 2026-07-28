using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class EdgeCodec : Codec
{
    /* Constants. */
    public const string TAG = "edge";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID, From, Port, To];

    /* Constructors. */
    public EdgeCodec(XmlNode xml) : base(xml) { }
}