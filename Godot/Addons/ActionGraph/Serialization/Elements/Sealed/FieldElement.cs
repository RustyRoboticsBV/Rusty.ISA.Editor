////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;
using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A field inspector, which represents the state of an instruction argument.
/// </summary>
internal sealed class FieldElement : ArgumentElement
{
    /* Public properties. */
    public override int AttributeCount => 5;
    
    /// <summary>
    /// The unique identifier of this field.
    /// </summary>
    public override string ID { get; set; }
    /// <summary>
    /// The identifier of the associated field definition.
    /// </summary>
    public override string Definition { get; set; }
    /// <summary>
    /// The non-localized value of this field.
    /// </summary>
    public string Value { get; set; }
    /// <summary>
    /// The annotation of this field.
    /// </summary>
    public override string Text { get; set; }
    /// <summary>
    /// The color of this field.
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
                return Value;
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
                Value = (string)value;
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
            case LocalizedValueElement:
                Children.Add(child);
                break;
            default:
                throw new ArgumentOutOfRangeException($"Elements of type {GetType().Name} do not allow children of type {child.GetType().Name}.");
        }
    }
}