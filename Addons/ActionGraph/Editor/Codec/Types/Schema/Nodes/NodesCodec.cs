using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class NodesCodec : Codec
{
    /* Constants. */
    public const string TAG = "nodes";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [NdefCodec.TAG];

    /* Constructors. */
    public NodesCodec(XmlNode xml) : base(xml) { }
}