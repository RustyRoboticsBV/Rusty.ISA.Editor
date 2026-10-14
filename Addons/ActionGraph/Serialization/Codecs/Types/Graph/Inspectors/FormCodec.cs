using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class FormCodec : InspectorCodec
{
    /* Constants. */
    public const string TAG = "form";
    public override string Name => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Definition];
    protected override List<string> AllowedChildren => [ArgCodec.TAG, OutCodec.TAG];

    /* Constructors. */
    public FormCodec() : base() { }
}