////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated file. It should not be edited directly.                //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A definition for an option inspector.
/// </summary>
internal sealed class ListDefinitionElement : Element, IInspectorDefinitionElement
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 9;
    public const string ELEMENT_NAME = "list_definition";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 1;
    /// <summary>
    /// The unique identifier of this list definition.
    /// </summary>
    public override string ID { get; set; } = "";
    
    public override bool AllowsChildren => true;
    
    /* Public methods. */
    public override int IndexOfAttribute(string name) => name switch
    {
        "id" => 0,
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.")
    };
    
    
    public override string NameOfAttribute(int index) => index switch
    {
        0 => "id",
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override Type TypeOfAttribute(int index) => index switch
    {
        0 => ID.GetType(),
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override object GetAttribute(int index) => index switch
    {
        0 => ID,
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override void SetAttribute(int index, object value)
    {
        switch (index)
        {
            case 0:
                ID = (string)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override void AddChild(Element child)
    {
        if (Children.Count > 0)
            throw new InvalidOperationException($"Elements of type '{ElementName}' cannot have multiple child elements.");
        
        switch (child)
        {
            case FormDefinitionElement:
            case OptionDefinitionElement:
            case ChoiceDefinitionElement:
            case TupleDefinitionElement:
            case ListDefinitionElement:
                Children.Add(child);
                break;
            default:
                throw new ArgumentOutOfRangeException($"Elements of type '{ElementName}' cannot have children of type '{child.ElementName}'.");
        }
    }
}