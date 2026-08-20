using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A base class for inspector codecs.
/// </summary>
internal abstract class InspectorCodec : Codec
{
    /* Constructors. */
    public InspectorCodec(XmlNode xml) : base(xml) { }
}