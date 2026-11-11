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
internal sealed class FormDefinitionElement : Element, IInspectorDefinitionElement
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 10;
    public const string ELEMENT_NAME = "form_definition";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 2;
    /// <summary>
    /// The unique identifier of this form definition.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The identifier of the associated instruction definition.
    /// </summary>
    public string Instruction { get; set; } = "";
    
    public override bool AllowsChildren => true;
    
    /* Public methods. */
    public override int IndexOfAttribute(string name) => name switch
    {
        "id" => 0,
        "instruction" => 1,
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.")
    };
    
    
    public override string NameOfAttribute(int index) => index switch
    {
        0 => "id",
        1 => "instruction",
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override Type TypeOfAttribute(int index) => index switch
    {
        0 => ID.GetType(),
        1 => Instruction.GetType(),
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override object GetAttribute(int index) => index switch
    {
        0 => ID,
        1 => Instruction,
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override void SetAttribute(int index, object value)
    {
        switch (index)
        {
            case 0:
                ID = (string)value;
                break;
            case 1:
                Instruction = (string)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override void AddChild(Element child)
    {
        switch (child)
        {
            case FieldDefinitionElement:
            case PortDefinitionElement:
                Children.Add(child);
                break;
            default:
                throw new ArgumentOutOfRangeException($"Elements of type '{ElementName}' cannot have children of type '{child.ElementName}'.");
        }
    }
}