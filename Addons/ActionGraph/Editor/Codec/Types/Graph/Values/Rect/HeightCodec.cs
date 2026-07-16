using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class HeightCodec : Codec
{
    /* Constants. */
    public const string TAG = "height";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public HeightCodec(XmlNode xml) : base(xml) { }
}