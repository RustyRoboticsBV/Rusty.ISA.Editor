using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

internal sealed class IdefCodec : Codec
{
    /* Constants. */
    public const string TAG = "idef";
    public override string Tag => TAG;

    /* Public properties. */
    protected override List<string> AllowedAttributes => [Codecs.ID, Codecs.Exec];
    protected override List<string> AllowedChildren => [PdefCodec.TAG];

    /* Constructors. */
    public IdefCodec() : base() { }

    /* Public methods. */
    /// <summary>
    /// Find a PdefCodec with some ID. Returns null if it doesn't exist.
    /// </summary>
    public PdefCodec FindPdef(string id)
    {
        foreach (Codec child in Children)
        {
            if (child is PdefCodec pdef && pdef.GetAttribute(Codecs.ID) == id)
                return pdef;
        }
        return null;
    }
}