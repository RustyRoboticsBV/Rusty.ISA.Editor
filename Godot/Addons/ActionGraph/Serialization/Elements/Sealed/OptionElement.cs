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
internal sealed class OptionElement : Element, IInspectorElement
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 16;
    public const string ELEMENT_NAME = "option";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 4;
    /// <summary>
    /// The unique identifier of this option.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The identifier of the associated option definition.
    /// </summary>
    public string Definition { get; set; } = "";
    /// <summary>
    /// Whether this option has been enabled.
    /// </summary>
    public bool Enabled { get; set; }
    /// <summary>
    /// Whether this option has been folded.
    /// </summary>
    public bool Folded { get; set; }
    
    public override bool AllowsChildren => true;
    
    /* Public methods. */
    public override int IndexOfAttribute(string name) => name switch
    {
        "id" => 0,
        "definition" => 1,
        "enabled" => 2,
        "folded" => 3,
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.")
    };
    
    
    public override string NameOfAttribute(int index) => index switch
    {
        0 => "id",
        1 => "definition",
        2 => "enabled",
        3 => "folded",
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override Type TypeOfAttribute(int index) => index switch
    {
        0 => ID.GetType(),
        1 => Definition.GetType(),
        2 => Enabled.GetType(),
        3 => Folded.GetType(),
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override object GetAttribute(int index) => index switch
    {
        0 => ID,
        1 => Definition,
        2 => Enabled,
        3 => Folded,
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
                Definition = (string)value;
                break;
            case 2:
                Enabled = (bool)value;
                break;
            case 3:
                Folded = (bool)value;
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
            case FormElement:
            case OptionElement:
            case ChoiceElement:
            case TupleElement:
            case ListElement:
                Children.Add(child);
                break;
            default:
                throw new ArgumentOutOfRangeException($"Elements of type '{ElementName}' cannot have children of type '{child.ElementName}'.");
        }
    }
}