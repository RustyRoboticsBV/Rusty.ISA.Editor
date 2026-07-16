using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class IdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "idef";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["id"];
    protected override HashSet<string> AllowedChildren => [PdefCodec.TAG, ExecCodec.TAG];

    /* Constructors. */
    public IdefCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public static void Register() => Codecs.Add(TAG, typeof(IdefCodec));
}