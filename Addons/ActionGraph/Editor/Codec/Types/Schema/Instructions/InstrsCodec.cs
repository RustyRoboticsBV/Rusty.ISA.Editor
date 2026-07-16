using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class InstrsCodec : Codec
{
    /* Constants. */
    public const string TAG = "instrs";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [IdefCodec.TAG];

    /* Constructors. */
    public InstrsCodec(XmlNode xml) : base(xml) { }
}