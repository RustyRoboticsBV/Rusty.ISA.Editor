////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;
using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A node output port, which represents the state of an instruction argument.
/// </summary>
internal sealed class PortElement : ArgumentElement
{
    /* Public properties. */
    public const int ELEMENT_INDEX = 24;
    public const string ELEMENT_NAME = "port";
    
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    public override int AttributeCount => 5;
    
    /// <summary>
    /// The unique identifier of this port.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The identifier of the associated port definition.
    /// </summary>
    public override string Definition { get; set; } = "";
    /// <summary>
    /// The index of this port on its parent node.
    /// </summary>
    public uint Index { get; set; }
    /// <summary>
    /// The annotation of this port port.
    /// </summary>
    public override string Text { get; set; } = "";
    /// <summary>
    /// The color of this port port.
    /// </summary>
    public override Color Color { get; set; }
    
    /* Public methods. */
    public override int IndexOfAttribute(string name)
    {
        switch (name)
        {
            case "id":
                return 0;
            case "definition":
                return 1;
            case "index":
                return 2;
            case "text":
                return 3;
            case "color":
                return 4;
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
                return "index";
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
                return Index.GetType();
            case 3:
                return Text.GetType();
            case 4:
                return Color.GetType();
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
                return Index;
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
                Index = (uint)value;
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
    
    public override void AddChild(Element child) => throw new NotImplementedException($"Elements of type {GetType().Name} do not allow child elements.");
}