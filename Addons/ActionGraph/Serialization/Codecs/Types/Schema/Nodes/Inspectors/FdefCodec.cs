using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class FdefCodec : InspectorDefinitionCodec
{
    /* Constants. */
    public const string TAG = "fdef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID, Type];
    protected override HashSet<string> AllowedChildren => [VadefCodec.TAG, OadefCodec.TAG];

    /* Constructors. */
    public FdefCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    /// <summary>
    /// Try to find an output definition codec.
    /// </summary>
    public OadefCodec FindOadef(string id)
    {
        foreach (Codec child in Children)
        {
            if (child is OadefCodec oadef && oadef.GetAttribute(ID) == id)
                return oadef;
        }
        return null;
    }
}