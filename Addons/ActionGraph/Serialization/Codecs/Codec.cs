using System;
using System.Collections.Generic;
using System.Xml;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A serializer codec.
/// </summary>
internal abstract class Codec
{
    /* Public properties. */
    public abstract string Tag { get; }
    public List<Codec> Children { get; } = new();
    public Dictionary<string, string> Attributes { get; } = new();

    /* Protected properties. */
    protected virtual List<string> AllowedChildren { get; } = new();
    protected virtual List<string> AllowedAttributes { get; } = new();

    /* Constructors. */
    public Codec() { }

    public Codec(XmlNode xml)
    {
        if (xml.ChildNodes.Count > 0)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                    AddChild(Codecs.Instantiate(element));
            }
        }

        foreach (XmlAttribute attribute in xml.Attributes)
        {
            Attributes.TryAdd(attribute.Name, attribute.Value);
        }
    }

    /* Public methods. */
    public override string ToString() => CodecPrinter.ToString(this, false);

    /// <summary>
    /// Check whether or not this codec supports attributes.
    /// </summary>
    public bool AllowsAttributes() => AllowedAttributes.Count > 0;

    /// <summary>
    /// Check whether or not an attribute with some name is allowed by this codec.
    /// </summary>
    public bool AllowsAttribute(string name) => AllowedAttributes.Contains(name);

    /// <summary>
    /// Set an attribute's value.
    /// </summary>
    public void SetAttribute(string name, string value)
    {
        if (!AllowsAttribute(name))
            throw new InvalidOperationException($"Codec '{GetType().Name}' cannot have an attribute with name '{name}'.");
        Attributes[name] = value;
    }

    /// <summary>
    /// Get an attribute's value. Returns "" if the attribute could not be found.
    /// </summary>
    public string GetAttribute(string name)
    {
        if (Attributes.TryGetValue(name, out var attribute))
            return attribute;
        return "";
    }

    /// <summary>
    /// Check whether or not this codec supports children.
    /// </summary>
    public bool AllowsChildren() => AllowedChildren.Count > 0;

    /// <summary>
    /// Check whether or not a child with some tag is allowed by this codec.
    /// </summary>
    public bool AllowsChild(string tag) => AllowedChildren.Contains(tag);

    /// <summary>
    /// Get the index of an attribute.
    /// </summary>
    public int GetAttributeIndex(string name) => AllowedAttributes.IndexOf(name);

    /// <summary>
    /// Get the attribute name corresponding to some index.
    /// </summary>
    public string GetAttributeFromIndex(int index) => AllowedAttributes[index];

    /// <summary>
    /// Add a node of some type.
    /// </summary>
    public void AddChild(Codec node)
    {
        if (!AllowsChild(node.Tag))
            throw new InvalidOperationException($"Codec '{GetType().Name}' cannot have a child with tag '{node.GetType().Name}'.");
        Children.Add(node);
    }

    /// <summary>
    /// Get the first child node with some tag. Returns null if the child does not exist.
    /// </summary>
    public T GetFirstChild<T>()
        where T : Codec
    {
        foreach (Codec child in Children)
        {
            if (child is T typed)
                return typed;
        }
        return null;
    }

    /// <summary>
    /// Find the first child codec with a specific attribute value.
    /// </summary>
    public Codec FindChildWithAttribute(string attributeName, string attributeValue)
    {
        foreach (Codec child in Children)
        {
            if (child.GetAttribute(attributeName) == attributeValue)
                return child;
        }
        return null;
    }
}