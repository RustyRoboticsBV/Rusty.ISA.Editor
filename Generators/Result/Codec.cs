using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

public abstract class Codec
{
    /* Public properties. */
    public abstract int AttributeCount { get; }
    public int ChildCount => Children.Count;

    protected List<Codec> Children { get; } = new();

    public abstract object GetAttribute(int index);
    public abstract void SetAttribute(int index, object value);

    public abstract void AddChild(Codec child);
    public void RemoveChild(int index) => Children.RemoveAt(index);
}