using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class MetaCodec : Codec
{
    /* Constants. */
    public const string TAG = "meta";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID, Value];

    /* Constructors. */
    public MetaCodec(XmlNode xml) : base(xml) { }
}