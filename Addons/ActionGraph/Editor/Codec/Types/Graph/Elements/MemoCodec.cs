using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class MemoCodec : Codec
{
    /* Constants. */
    public const string TAG = "memo";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["id"];
    protected override HashSet<string> AllowedChildren => ["x", "y", "member", "text", "color"];

    /* Constructors. */
    public MemoCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public static void Register() => Codecs.Add(TAG, typeof(MemoCodec));
}