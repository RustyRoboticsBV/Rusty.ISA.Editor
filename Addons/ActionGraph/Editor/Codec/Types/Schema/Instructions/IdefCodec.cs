using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class IdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "idef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [PdefCodec.TAG, ExecCodec.TAG];

    /* Constructors. */
    public IdefCodec(XmlNode xml) : base(xml) { }
}