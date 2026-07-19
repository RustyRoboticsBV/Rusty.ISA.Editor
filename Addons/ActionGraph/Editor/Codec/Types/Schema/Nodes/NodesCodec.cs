using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class NodesCodec : Codec
{
    /* Constants. */
    public const string TAG = "nodes";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [NdefCodec.TAG];

    /* Constructors. */
    public NodesCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    /// <summary>
    /// Find a node definition with some ID.
    /// </summary>
    public NdefCodec FindNode(string id)
    {
        foreach (Codec child in Children)
        {
            if (child is NdefCodec ndef && ndef.GetAttribute(ID) == id)
                return ndef;
        }
        return null;
    }
}