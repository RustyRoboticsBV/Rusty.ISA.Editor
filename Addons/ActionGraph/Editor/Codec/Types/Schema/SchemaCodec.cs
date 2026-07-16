using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class SchemaCodec : Codec
{
    /* Constants. */
    public const string TAG = "schema";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [InstrsCodec.TAG, NodesCodec.TAG];

    /* Constructors. */
    public SchemaCodec(XmlNode xml) : base(xml) { }
}