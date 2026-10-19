using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A serializer codec.
/// </summary>
public abstract class Codec
{
    /* Public properties. */
    /// <summary>
    /// The identifier of this codec.
    /// </summary>
    public abstract string ID { get; set; }

    /// <summary>
    /// The number of attributes.
    /// </summary>
    public abstract int AttributeCount { get; }
    /// <summary>
    /// The number of child codecs.
    /// </summary>
    public int ChildCount => Children.Count;

    /* Protected properties. */
    protected List<Codec> Children { get; } = new();

    /* Public methods. */
    public override string ToString() => CodecPrinter.ToString(this, false);

    /// <summary>
    /// Get an attribute value.
    /// </summary>
    public abstract object GetAttribute(int index);
    /// <summary>
    /// Set an attribute's value.
    /// </summary>
    public abstract void SetAttribute(int index, object value);

    /// <summary>
    /// Add a child codec.
    /// </summary>
    public abstract void AddChild(Codec child);
    /// <summary>
    /// Get a child codec.
    /// </summary>
    public Codec GetChild(int index) => Children[index];
    /// <summary>
    /// Remove a child codec.
    /// </summary>
    public void RemoveChild(int index) => Children.RemoveAt(index);
}