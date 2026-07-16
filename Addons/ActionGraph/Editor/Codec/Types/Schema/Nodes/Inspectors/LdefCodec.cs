using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class LdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "ldef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["id"];
    protected override HashSet<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, CdefCodec.TAG, TdefCodec.TAG, TAG];

    /* Constructors. */
    public LdefCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public static void Register() => Codecs.Add(TAG, typeof(LdefCodec));
}