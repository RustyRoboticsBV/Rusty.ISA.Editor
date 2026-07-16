using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class NdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "ndef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["id"];
    protected override HashSet<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, CdefCodec.TAG, TdefCodec.TAG, LdefCodec.TAG];

    /* Constructors. */
    public NdefCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public static void Register() => Codecs.Add(TAG, typeof(NdefCodec));
}