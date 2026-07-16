using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class ArgCodec : Codec
{
    /* Constants. */
    public const string TAG = "arg";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [Type];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public ArgCodec(XmlNode xml) : base(xml) { }
}