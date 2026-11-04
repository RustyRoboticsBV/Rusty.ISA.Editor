////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;
using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A choice inspector.
/// </summary>
internal sealed class ChoiceElement : InspectorElement
{
    /* Public properties. */
    public override int AttributeCount => 6;
    
    /// <summary>
    /// The unique identifier of this choice.
    /// </summary>
    public override string ID { get; set; }
    /// <summary>
    /// The identifier of the associated choice definition.
    /// </summary>
    public override string Definition { get; set; }
    /// <summary>
    /// The identifier of the selected item.
    /// </summary>
    public string Selected { get; set; }
    /// <summary>
    /// Whether the choice has been folded.
    /// </summary>
    public override bool Folded { get; set; }
    /// <summary>
    /// The annotation of this choice.
    /// </summary>
    public override string Text { get; set; }
    /// <summary>
    /// The color of this choice.
    /// </summary>
    public override Color Color { get; set; }
    
    /* Public methods. */
    public override object GetAttribute(int index)
    {
        switch (index)
        {
            case 0:
                return ID;
            case 1:
                return Definition;
            case 2:
                return Selected;
            case 3:
                return Folded;
            case 4:
                return Text;
            case 5:
                return Color;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to index {index}.");
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
                Selected = (string)value;
                break;
            case 3:
                Folded = (bool)value;
                break;
            case 4:
                Text = (string)value;
                break;
            case 5:
                Color = (Color)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to index {index}.");
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