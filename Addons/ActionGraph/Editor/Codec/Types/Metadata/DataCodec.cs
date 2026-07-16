using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class DataCodec : Codec
{
    /* Constants. */
    public const string TAG = "data";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public DataCodec(XmlNode xml) : base(xml) { }
}