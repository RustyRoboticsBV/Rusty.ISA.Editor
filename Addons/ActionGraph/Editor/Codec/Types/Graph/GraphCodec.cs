using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class GraphCodec : Codec
{
    /* Constants. */
    public const string TAG = "graph";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [ElemsCodec.TAG, EdgesCodec.TAG];

    /* Constructors. */
    public GraphCodec(XmlNode xml) : base(xml) { }
}