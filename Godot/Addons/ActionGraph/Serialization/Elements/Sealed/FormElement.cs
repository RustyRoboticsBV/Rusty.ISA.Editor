////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;
using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A form inspector, which represennts the state of an instruction.
/// </summary>
internal sealed class FormElement : InspectorElement
{
    /* Public properties. */
    public const int ELEMENT_INDEX = 18;
    public const string ELEMENT_NAME = "form";
    
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    public override int AttributeCount => 5;
    
    /// <summary>
    /// The unique identifier of this form.
    /// </summary>
    public override string ID { get; set; }
    /// <summary>
    /// The identifier of the associated form definition.
    /// </summary>
    public override string Definition { get; set; }
    /// <summary>
    /// Whether the form has been folded.
    /// </summary>
    public override bool Folded { get; set; }
    /// <summary>
    /// The annotation of this form.
    /// </summary>
    public override string Text { get; set; }
    /// <summary>
    /// The color of this form.
    /// </summary>
    public override Color Color { get; set; }
    
    /* Public methods. */
    public override string NameOfAttribute(int index)
    {
        switch (index)
        {
            case 0:
                return "id";
            case 1:
                return "definition";
            case 2:
                return "folded";
            case 3:
                return "text";
            case 4:
                return "color";
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
                return Folded.GetType();
            case 3:
                return Text.GetType();
            case 4:
                return Color.GetType();
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override int IndexOfAttribute(string name)
    {
        switch (name)
        {
            case "id":
                return 5;
            case "definition":
                return 5;
            case "folded":
                return 5;
            case "text":
                return 5;
            case "color":
                return 5;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.");
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
                return Folded;
            case 3:
                return Text;
            case 4:
                return Color;
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
                Folded = (bool)value;
                break;
            case 3:
                Text = (string)value;
                break;
            case 4:
                Color = (Color)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override void AddChild(Element child)
    {
        switch (child)
        {
            case FieldElement:
            case PortElement:
                Children.Add(child);
                break;
            default:
                throw new ArgumentOutOfRangeException($"Elements of type {GetType().Name} do not allow children of type {child.GetType().Name}.");
        }
    }
}