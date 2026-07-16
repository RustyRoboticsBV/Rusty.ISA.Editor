using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class CdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "cdef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["id"];
    protected override HashSet<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, TAG, TdefCodec.TAG, LdefCodec.TAG];

    /* Constructors. */
    public CdefCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public static void Register() => Codecs.Add(TAG, typeof(CdefCodec));
}