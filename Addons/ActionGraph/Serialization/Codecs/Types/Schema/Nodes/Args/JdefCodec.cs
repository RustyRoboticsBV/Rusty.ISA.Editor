using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class JdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "jdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID, Type, NoDefault];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public JdefCodec(XmlNode xml) : base(xml) { }
}