using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class MetaCodec : Codec
{
    /* Constants. */
    public const string TAG = "meta";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID, Value];

    /* Constructors. */
    public MetaCodec(XmlNode xml) : base(xml) { }
}