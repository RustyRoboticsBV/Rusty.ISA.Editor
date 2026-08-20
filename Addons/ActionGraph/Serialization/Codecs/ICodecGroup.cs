using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A codec that represents a group of child codecs.
/// </summary>
internal interface ICodecGroup<T>
    where T : Codec
{
    /* Public properties. */
    public List<Codec> Children { get; }

    /* Public methods. */
    /// <summary>
    /// Try to find a child codec of some type with an ID. Returns null if the child codec cannot be found.
    /// </summary>
    public T Find(string id)
    {
        foreach (Codec child in Children)
        {
            if (child is T typed && typed.GetAttribute(Codec.ID) == id)
                return typed;
        }
        return null;
    }
}