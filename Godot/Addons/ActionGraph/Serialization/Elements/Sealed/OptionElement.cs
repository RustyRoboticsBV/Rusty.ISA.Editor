////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// An option inspector.
/// </summary>
internal sealed class OptionElement : InspectorElement
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 23;
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
    public override string Definition { get; set; } = "";
    /// <summary>
    /// Whether this option has been enabled.
    /// </summary>
    public bool Enabled { get; set; }
    /// <summary>
    /// Whether this option has been folded.
    /// </summary>
    public override bool Folded { get; set; }
    
    public override bool AllowsChildren => true;
    
    /* Public methods. */
    public override int IndexOfAttribute(string name)
    {
        switch (name)
        {
            case "id":
                return 0;
            case "definition":
                return 1;
            case "enabled":
                return 2;
            case "folded":
                return 3;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.");
        }
    }
    
    
    public override string NameOfAttribute(int index)
    {
        switch (index)
        {
            case 0:
                return "id";
            case 1:
                return "definition";
            case 2:
                return "enabled";
            case 3:
                return "folded";
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override Type TypeOfAttribute(int index)
    {
        switch (index)
        {
            case 0:
                return ID.GetType();
            case 1:
                return Definition.GetType();
            case 2:
                return Enabled.GetType();
            case 3:
                return Folded.GetType();
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override object GetAttribute(int index)
    {
        switch (index)
        {
            case 0:
                return ID;
            case 1:
                return Definition;
            case 2:
                return Enabled;
            case 3:
                return Folded;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
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
            throw new InvalidOperationException($"Elements of type {GetType().Name} do not allow multiple child elements.");
        
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
                throw new ArgumentOutOfRangeException($"Elements of type {GetType().Name} do not allow children of type {child.GetType().Name}.");
        }
    }
}