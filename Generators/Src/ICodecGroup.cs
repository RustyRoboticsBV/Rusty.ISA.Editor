namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A codec that represents a group of child codecs.
/// </summary>
internal interface ICodecGroup<CodecT>
    where CodecT : Codec
{
    /* Public methods. */
    /// <summary>
    /// Try to find a child codec of a specific type and ID. Returns null if the child codec cannot be found.
    /// </summary>
    public CodecT FindChild(string id);
}