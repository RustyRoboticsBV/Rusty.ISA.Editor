using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed partial class ColorCodec : Codec
{
    /* Constants. */
    public const string TAG = "color";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => [];
    protected override HashSet<string> AllowedChildren => [];

    /* Constructors. */
    public ColorCodec(XmlNode xml) : base(xml) { }

    /* Public methods. */
    public static void Register() => Codecs.Add(TAG, typeof(ColorCodec));
}