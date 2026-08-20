using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class PdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "pdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID, Localizable];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public PdefCodec(XmlNode xml) : base(xml) { }
}