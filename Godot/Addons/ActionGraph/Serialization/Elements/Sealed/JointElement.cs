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
internal sealed class JointElement : Element
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 30;
    public const string ELEMENT_NAME = "joint";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 6;
    /// <summary>
    /// The unique identifier of this joint.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The x-position of this joint.
    /// </summary>
    public int X { get; set; }
    /// <summary>
    /// The y-position of this joint.
    /// </summary>
    public int Y { get; set; }
    /// <summary>
    /// The size of this joint.
    /// </summary>
    public uint Size { get; set; }
    /// <summary>
    /// The identifier of the parent frame.
    /// </summary>
    public string ParentFrame { get; set; } = "";
    /// <summary>
    /// The color of this joint.
    /// </summary>
    public Color Color { get; set; }
    
    public override bool AllowsChildren => false;
    
    /* Public methods. */
    public override int IndexOfAttribute(string name) => name switch
    {
        "id" => 0,
        "x" => 1,
        "y" => 2,
        "size" => 3,
        "parent_frame" => 4,
        "color" => 5,
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.")
    };
    
    
    public override string NameOfAttribute(int index) => index switch
    {
        0 => "id",
        1 => "x",
        2 => "y",
        3 => "size",
        4 => "parent_frame",
        5 => "color",
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override Type TypeOfAttribute(int index) => index switch
    {
        0 => ID.GetType(),
        1 => X.GetType(),
        2 => Y.GetType(),
        3 => Size.GetType(),
        4 => ParentFrame.GetType(),
        5 => Color.GetType(),
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override object GetAttribute(int index) => index switch
    {
        0 => ID,
        1 => X,
        2 => Y,
        3 => Size,
        4 => ParentFrame,
        5 => Color,
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
                X = (int)value;
                break;
            case 2:
                Y = (int)value;
                break;
            case 3:
                Size = (uint)value;
                break;
            case 4:
                ParentFrame = (string)value;
                break;
            case 5:
                Color = (Color)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override void AddChild(Element child)
         => throw new NotImplementedException($"Elements of type '{ElementName}' cannot have child elements.");
}