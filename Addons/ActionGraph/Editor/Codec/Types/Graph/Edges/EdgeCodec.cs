using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class EdgeCodec : Codec
{
    /* Constants. */
    public const string TAG = "edge";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID, From, Port, To];

    /* Constructors. */
    public EdgeCodec(XmlNode xml) : base(xml) { }
}