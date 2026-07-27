using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A base class for inspector definition codecs.
/// </summary>
public abstract partial class InspectorDefinitionCodec : Codec
{
    /* Constructors. */
    public InspectorDefinitionCodec(XmlNode xml) : base(xml) { }
}