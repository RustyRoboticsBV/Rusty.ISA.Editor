using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class WidthCodec : Codec
{
    /* Constants. */
    public const string TAG = "width";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public WidthCodec(XmlNode xml) : base(xml) { }
}