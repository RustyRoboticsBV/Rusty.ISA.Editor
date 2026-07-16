using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class StartCodec : Codec
{
    /* Constants. */
    public const string TAG = "start";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["id"];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public StartCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public static void Register() => Codecs.Add(TAG, typeof(StartCodec));
}