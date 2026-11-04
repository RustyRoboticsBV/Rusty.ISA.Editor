////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;
using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A tuple inspector.
/// </summary>
internal sealed class TupleElement : InspectorElement
{
    /* Public properties. */
    public override int AttributeCount => 5;
    
    /// <summary>
    /// The unique identifier of this tuple.
    /// </summary>
    public override string ID { get; set; }
    /// <summary>
    /// The identifier of the associated tuple definition.
    /// </summary>
    public override string Definition { get; set; }
    /// <summary>
    /// Whether the tuple has been folded.
    /// </summary>
    public override bool Folded { get; set; }
    /// <summary>
    /// The annotation of this tuple.
    /// </summary>
    public override string Text { get; set; }
    /// <summary>
    /// The color of this tuple.
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
                return Folded;
            case 3:
                return Text;
            case 4:
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
                Folded = (bool)value;
                break;
            case 3:
                Text = (string)value;
                break;
            case 4:
                Color = (Color)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to index {index}.");
        }
    }
    
    public override void AddChild(Element child)
    {
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