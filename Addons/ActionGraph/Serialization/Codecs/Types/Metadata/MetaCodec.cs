using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A codec representing a metadata record.
/// </summary>
internal sealed class MetaCodec : Codec
{
    /* Constants. */
    public const string TAG = "meta";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Value];

    /* Constructors. */
    public MetaCodec() : base() { }

    public MetaCodec(XmlNode xml) : base(xml) { }
}