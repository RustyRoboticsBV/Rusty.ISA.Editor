using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class PdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "pdef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public PdefCodec(XmlNode xml) : base(xml) { }
}