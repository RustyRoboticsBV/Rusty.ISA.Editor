public sealed class BaseElementClassGenerator : Generator
{
    public string Generate(Schema schema)
    {
        // Get ID names.
        string id = schema.GlobalAttributes[0].Name;
        string idCaps = Names.CamelCase(id);

        // Generate global attribute properties.
        CodeWriter globals = new();
        globals.Indent++;
        foreach (Attribute shared in schema.GlobalAttributes)
        {
            globals.Line($"// <summary>");
            globals.Line($"// {shared.Description}");
            globals.Line($"// </summary>");
            globals.Line($"public abstract string {Names.CamelCase(shared.Name)} {{ get; set; }}");
        }

        // Generate script.
        return $@"{Notice}

using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A serializer element, representing a node in an ActionGraph document tree.
/// </summary>
internal abstract class Element
{{
    /* Public properties. */
    /// <summary>
    /// The index of this element type.
    /// </summary>
    public abstract int ElementIndex {{ get; }}
    /// <summary>
    /// The name of this element type.
    /// </summary>
    public abstract string ElementName {{ get; }}
    
    /// <summary>
    /// The number of attributes of this element.
    /// </summary>
    public abstract int AttributeCount {{ get; }}
{globals}
    
    /// <summary>
    /// Whether or not this element can have child elements.
    /// </summary>
    public abstract bool AllowsChildren {{ get; }}
    /// <summary>
    /// The number of child elements contained within this element.
    /// </summary>
    public int ChildCount => Children.Count;
    
    /* Protected properties. */
    /// <summary>
    /// The child elements.
    /// </summary>
    protected List<Element> Children {{ get; }} = new();
    
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
    /// Add a child element.
    /// </summary>
    public abstract void AddChild(Element child);
    
    /// <summary>
    /// Get a child element by its index.
    /// </summary>
    public Element GetChild(int index)
    {{
        if (index < 0 || index >= ChildCount)
            throw new ArgumentOutOfRangeException($""The child index {{index}} was out of range (0-{{ChildCount}})."");
        return Children[index];
    }}

    /// <summary>
    /// Get the first child element of the return type. Returns null if the child element cannot be found.
    /// </summary>
    public ElementT Find<ElementT>()
        where ElementT : Element
    {{
        for (int i = 0; i < ChildCount; i++)
        {{
            if (GetChild(i) is ElementT child)
                return child;
        }}
        return null;
    }}

    /// <summary>
    /// Get a child element of the return type with a specific {idCaps}. Returns null if the child element cannot be found.
    /// </summary>
    public ElementT Find<ElementT>(string {id})
        where ElementT : Element
    {{
        for (int i = 0; i < ChildCount; i++)
        {{
            if (GetChild(i) is ElementT child && child.{idCaps} == {id})
                return child;
        }}
        return null;
    }}
}}";
    }
}