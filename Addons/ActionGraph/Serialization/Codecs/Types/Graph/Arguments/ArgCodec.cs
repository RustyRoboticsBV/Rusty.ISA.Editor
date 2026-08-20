using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class ArgCodec : Codec
{
    /* Constants. */
    public const string TAG = "arg";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [Type, Value];

    /* Constructors. */
    public ArgCodec(XmlNode xml) : base(xml) { }
}