using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class FdefCodec : InspectorDefinitionCodec
{
    /* Constants. */
    public const string TAG = "fdef";

    /* Public properties. */
    protected override string Tag => TAG;
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
        foreach (OadefCodec codec in GetChildren<OadefCodec>())
        {
            if (codec.GetAttribute(ID) == id)
                return codec;
        }
        return null;
    }
}