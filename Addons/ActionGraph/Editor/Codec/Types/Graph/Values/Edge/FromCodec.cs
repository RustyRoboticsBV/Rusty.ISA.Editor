using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class FromCodec : Codec
{
    /* Constants. */
    public const string TAG = "from";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["id"];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public FromCodec(XmlNode xml) : base(xml) { }
}