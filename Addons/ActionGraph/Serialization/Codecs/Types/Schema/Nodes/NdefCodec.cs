using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class NdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "ndef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, CdefCodec.TAG, TdefCodec.TAG, LdefCodec.TAG];

    /* Constructors. */
    public NdefCodec(XmlNode xml) : base(xml) { }

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