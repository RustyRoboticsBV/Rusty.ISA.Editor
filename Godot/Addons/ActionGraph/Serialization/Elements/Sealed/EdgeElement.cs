////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;
using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// An editor graph edge.
/// </summary>
internal sealed class EdgeElement : Element
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 18;
    public const string ELEMENT_NAME = "edge";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 6;
    /// <summary>
    /// The unique identifier of this edge.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The identifier of the source node.
    /// </summary>
    public string From { get; set; } = "";
    /// <summary>
    /// The port index on the source node.
    /// </summary>
    public uint Port { get; set; }
    /// <summary>
    /// The identifier of the destination node.
    /// </summary>
    public string To { get; set; } = "";
    /// <summary>
    /// The thickness of this edge.
    /// </summary>
    public uint Thickness { get; set; }
    /// <summary>
    /// The color of this edge.
    /// </summary>
    public Color Color { get; set; }
    
    public override bool AllowsChildren => true;
    
    /* Public methods. */
    public override int IndexOfAttribute(string name)
    {
        switch (name)
        {
            case "id":
                return 0;
            case "from":
                return 1;
            case "port":
                return 2;
            case "to":
                return 3;
            case "thickness":
                return 4;
            case "color":
                return 5;
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
                return "from";
            case 2:
                return "port";
            case 3:
                return "to";
            case 4:
                return "thickness";
            case 5:
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
                return From.GetType();
            case 2:
                return Port.GetType();
            case 3:
                return To.GetType();
            case 4:
                return Thickness.GetType();
            case 5:
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
                return From;
            case 2:
                return Port;
            case 3:
                return To;
            case 4:
                return Thickness;
            case 5:
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
                From = (string)value;
                break;
            case 2:
                Port = (uint)value;
                break;
            case 3:
                To = (string)value;
                break;
            case 4:
                Thickness = (uint)value;
                break;
            case 5:
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
            case JointElement:
                Children.Add(child);
                break;
            default:
                throw new ArgumentOutOfRangeException($"Elements of type {GetType().Name} do not allow children of type {child.GetType().Name}.");
        }
    }
}