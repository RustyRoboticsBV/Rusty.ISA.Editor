using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class OadefCodec : Codec
{
    /* Constants. */
    public const string TAG = "oadef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public OadefCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public static void Register() => Codecs.Add(TAG, typeof(OadefCodec));
}