using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class NdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "ndef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, CdefCodec.TAG, TdefCodec.TAG, LdefCodec.TAG];

    /* Constructors. */
    public NdefCodec(XmlNode xml) : base(xml) { }
}