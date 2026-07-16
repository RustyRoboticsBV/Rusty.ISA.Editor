using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class PortCodec : Codec
{
    /* Constants. */
    public const string TAG = "port";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["index"];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public PortCodec(XmlNode xml) : base(xml) { }
}