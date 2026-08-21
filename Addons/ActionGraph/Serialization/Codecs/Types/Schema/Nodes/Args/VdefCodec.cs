using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class VdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "vdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID, Type];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public VdefCodec(XmlNode xml) : base(xml) { }
}