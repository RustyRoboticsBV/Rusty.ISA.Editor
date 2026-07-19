using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

public sealed class FormCodec : InspectorCodec
{
    /* Constants. */
    public const string TAG = "form";

    /* Public properties. */
    protected override string Tag => TAG;
    protected override HashSet<string> AllowedAttributes => ["type"];
    protected override HashSet<string> AllowedChildren => [ArgCodec.TAG, OutCodec.TAG];

    /* Constructors. */
    public FormCodec(XmlNode xml) : base(xml) { }
}