using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class FormCodec : InspectorCodec
{
    /* Constants. */
    public const string TAG = "form";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.Type];
    protected override List<string> AllowedChildren => [ArgCodec.TAG, OutCodec.TAG];

    /* Constructors. */
    public FormCodec() : base() { }

    public FormCodec(XmlNode xml) : base(xml) { }
}