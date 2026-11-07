////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A serializer element, representing a node in the ActionGraph document tree.
/// </summary>
internal abstract class Element
{
    /* Public properties. */
    /// <summary>
    /// The index of this element type.
    /// </summary>
    public abstract int ElementIndex { get; }
    /// <summary>
    /// The name of this element type.
    /// </summary>
    public abstract string ElementName { get; }
    /// <summary>
    /// The number of attributes of this element.
    /// </summary>
    public abstract int AttributeCount { get; }
    /// <summary>
    /// The unique identifier of this element.
    /// </summary>
    public abstract string ID { get; set; }
    
    /// <summary>
    /// The number of child elements contained within this element.
    /// </summary>
    public int ChildCount => Children.Count;
    
    /* Protected properties. */
    /// <summary>
    /// The child elements.
    /// </summary>
    protected List<Element> Children { get; } = new();
    
    /* Public methods. */
    public sealed override string ToString() => ElementPrinter.ToString(this);
    
    /// <summary>
    /// Get the name of an attribute by its index.
    /// </summary>
    public abstract string NameOfAttribute(int index);
    
    /// <summary>
    /// Get the type of an attribute by its index.
    /// </summary>
    public abstract Type TypeOfAttribute(int index);
    
    /// <summary>
    /// Get the index of an attribute by its name.
    /// </summary>
    public abstract int IndexOfAttribute(string name);
    
    /// <summary>
    /// Get the value of an attribute by its index.
    /// </summary>
    public abstract object GetAttribute(int index);
    
    /// <summary>
    /// Set the value of an attribute by its index.
    /// </summary>
    public abstract void SetAttribute(int index, object value);
    
    /// <summary>
    /// Add a child node.
    /// </summary>
    public abstract void AddChild(Element child);
    
    /// <summary>
    /// Get a child node by its index.
    /// </summary>
    public Element GetChild(int index)
    {
        if (index < 0 || index >= ChildCount)
            throw new ArgumentOutOfRangeException($"The child index {index} was out of range (0-{ChildCount}).");
        return Children[index];
    }
}