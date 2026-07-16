using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class ExecCodec : Codec
{
    /* Constants. */
    public const string TAG = "exec";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public ExecCodec(XmlNode xml) : base(xml) { }
}