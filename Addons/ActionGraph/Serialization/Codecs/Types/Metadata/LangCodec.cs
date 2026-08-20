using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A codec representing a language definition.
/// </summary>
internal sealed class LangCodec : Codec
{
    /* Constants. */
    public const string TAG = "lang";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [ID];

    /* Constructors. */
    public LangCodec(XmlNode xml) : base(xml) { }
}