using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

internal sealed class FormCodec : InspectorCodec
{
    /* Constants. */
    public const string TAG = "form";
    public override string Tag => TAG;

    /* Public properties. */
    protected override HashSet<string> AllowedAttributes => [Type];
    protected override HashSet<string> AllowedChildren => [ArgCodec.TAG, OutCodec.TAG];

    /* Constructors. */
    public FormCodec(XmlNode xml) : base(xml) { }
}