using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class StartCodec : Codec
{
    /* Constants. */
    public const string TAG = "start";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public StartCodec(XmlNode xml) : base(xml) { }
}