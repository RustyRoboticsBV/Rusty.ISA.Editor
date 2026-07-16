using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class EdgesCodec : Codec
{
    /* Constants. */
    public const string TAG = "edges";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => ["edge"];

    /* Constructors. */
    public EdgesCodec(XmlNode xml) : base(xml) { }
}