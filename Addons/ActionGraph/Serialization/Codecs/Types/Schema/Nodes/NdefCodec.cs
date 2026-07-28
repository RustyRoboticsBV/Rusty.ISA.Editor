using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class NdefCodec : Codec, ICodecGroup<InspectorDefinitionCodec>
{
    /* Constants. */
    public const string TAG = "ndef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID];
    protected override HashSet<string> AllowedChildren => [FdefCodec.TAG, OdefCodec.TAG, CdefCodec.TAG, TdefCodec.TAG, LdefCodec.TAG];

    /* Constructors. */
    public NdefCodec(XmlNode xml) : base(xml) { }
}