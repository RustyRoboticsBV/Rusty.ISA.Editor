using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class OdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "odef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["id"];
    protected override HashSet<string> AllowedChildren => [FdefCodec.TAG, TAG, CdefCodec.TAG, TdefCodec.TAG, LdefCodec.TAG];

    /* Constructors. */
    public OdefCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public static void Register() => Codecs.Add(TAG, typeof(OdefCodec));
}