using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class ToCodec : Codec
{
    /* Constants. */
    public const string TAG = "to";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [Element];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public ToCodec(XmlNode xml) : base(xml) { }
}