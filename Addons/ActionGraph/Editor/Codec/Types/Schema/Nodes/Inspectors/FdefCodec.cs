using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class FdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "fdef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["id", "type"];
    protected override HashSet<string> AllowedChildren => [VadefCodec.TAG, OadefCodec.TAG];

    /* Constructors. */
    public FdefCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public static void Register() => Codecs.Add(TAG, typeof(FdefCodec));
}