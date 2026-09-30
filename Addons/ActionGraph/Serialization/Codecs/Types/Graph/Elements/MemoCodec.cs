using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class MemoCodec : Codec
{
    /* Constants. */
    public const string TAG = "memo";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.X, Codecs.Y, Codecs.Member, Codecs.Text, Codecs.Color];

    /* Constructors. */
    public MemoCodec() : base() { }

    public MemoCodec(XmlNode xml) : base(xml) { }
}