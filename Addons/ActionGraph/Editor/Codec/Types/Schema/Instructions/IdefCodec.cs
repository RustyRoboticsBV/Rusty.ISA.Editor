using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class IdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "idef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID, Exec];
    protected override HashSet<string> AllowedChildren => [PdefCodec.TAG];

    /* Constructors. */
    public IdefCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    /// <summary>
    /// Find a PdefCodec with some ID. Returns null if it doesn't exist.
    /// </summary>
    public PdefCodec FindPdef(string id)
    {
        foreach (Codec child in Children)
        {
            if (child is PdefCodec pdef && pdef.GetAttribute(ID) == id)
                return pdef;
        }
        return null;
    }
}