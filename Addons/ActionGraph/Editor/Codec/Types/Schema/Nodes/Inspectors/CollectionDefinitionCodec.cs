using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A base class for inspector definition codecs.
/// </summary>
public abstract partial class CollectionDefinitionCodec : InspectorCodec
{
    /* Constructors. */
    public CollectionDefinitionCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    /// <summary>
    /// Find a child InspectorDefinitionCodec with some ID. Returns null if it doesn't exist.
    /// </summary>
    public InspectorDefinitionCodec FindInspector(string id)
    {
        foreach (Codec child in Children)
        {
            if (child is InspectorDefinitionCodec inspector && inspector.GetAttribute(ID) == id)
                return inspector;
        }
        return null;
    }
}