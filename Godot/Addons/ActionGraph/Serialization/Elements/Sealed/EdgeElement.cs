////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated file. It should not be edited directly.                //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;
using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A definition for an option inspector.
/// </summary>
internal sealed class EdgeElement : Element
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 29;
    public const string ELEMENT_NAME = "edge";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 7;
    /// <summary>
    /// The unique identifier of this edge.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The identifier of the source element.
    /// </summary>
    public string From { get; set; } = "";
    /// <summary>
    /// The port index on the source element.
    /// </summary>
    public uint FromPort { get; set; }
    /// <summary>
    /// The identifier of the destination element.
    /// </summary>
    public string To { get; set; } = "";
    /// <summary>
    /// The port index of the destination element.
    /// </summary>
    public uint ToPort { get; set; }
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
    public override int IndexOfAttribute(string name) => name switch
    {
        "id" => 0,
        "from" => 1,
        "from_port" => 2,
        "to" => 3,
        "to_port" => 4,
        "thickness" => 5,
        "color" => 6,
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.")
    };
    
    
    public override string NameOfAttribute(int index) => index switch
    {
        0 => "id",
        1 => "from",
        2 => "from_port",
        3 => "to",
        4 => "to_port",
        5 => "thickness",
        6 => "color",
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override Type TypeOfAttribute(int index) => index switch
    {
        0 => ID.GetType(),
        1 => From.GetType(),
        2 => FromPort.GetType(),
        3 => To.GetType(),
        4 => ToPort.GetType(),
        5 => Thickness.GetType(),
        6 => Color.GetType(),
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override object GetAttribute(int index) => index switch
    {
        0 => ID,
        1 => From,
        2 => FromPort,
        3 => To,
        4 => ToPort,
        5 => Thickness,
        6 => Color,
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
                From = (string)value;
                break;
            case 2:
                FromPort = (uint)value;
                break;
            case 3:
                To = (string)value;
                break;
            case 4:
                ToPort = (uint)value;
                break;
            case 5:
                Thickness = (uint)value;
                break;
            case 6:
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
                throw new ArgumentOutOfRangeException($"Elements of type '{ElementName}' cannot have children of type '{child.ElementName}'.");
        }
    }
}