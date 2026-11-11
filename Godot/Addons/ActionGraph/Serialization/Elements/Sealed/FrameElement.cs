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
internal sealed class FrameElement : Element
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 25;
    public const string ELEMENT_NAME = "frame";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 9;
    /// <summary>
    /// The unique identifier of this frame.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The x-position of this frame on the graph.
    /// </summary>
    public int X { get; set; }
    /// <summary>
    /// The y-position of this frame on the graph.
    /// </summary>
    public int Y { get; set; }
    /// <summary>
    /// The width of this frame on the graph.
    /// </summary>
    public uint Width { get; set; }
    /// <summary>
    /// The height of this frame on the graph.
    /// </summary>
    public uint Height { get; set; }
    /// <summary>
    /// Whether this frame's inspector has been folded.
    /// </summary>
    public bool Folded { get; set; }
    /// <summary>
    /// The identifier of the parent frame.
    /// </summary>
    public string ParentFrame { get; set; } = "";
    /// <summary>
    /// The title text of this frame.
    /// </summary>
    public string Text { get; set; } = "";
    /// <summary>
    /// The color of this frame.
    /// </summary>
    public Color Color { get; set; }
    
    public override bool AllowsChildren => false;
    
    /* Public methods. */
    public override int IndexOfAttribute(string name) => name switch
    {
        "id" => 0,
        "x" => 1,
        "y" => 2,
        "width" => 3,
        "height" => 4,
        "folded" => 5,
        "parent_frame" => 6,
        "text" => 7,
        "color" => 8,
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.")
    };
    
    
    public override string NameOfAttribute(int index) => index switch
    {
        0 => "id",
        1 => "x",
        2 => "y",
        3 => "width",
        4 => "height",
        5 => "folded",
        6 => "parent_frame",
        7 => "text",
        8 => "color",
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override Type TypeOfAttribute(int index) => index switch
    {
        0 => ID.GetType(),
        1 => X.GetType(),
        2 => Y.GetType(),
        3 => Width.GetType(),
        4 => Height.GetType(),
        5 => Folded.GetType(),
        6 => ParentFrame.GetType(),
        7 => Text.GetType(),
        8 => Color.GetType(),
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override object GetAttribute(int index) => index switch
    {
        0 => ID,
        1 => X,
        2 => Y,
        3 => Width,
        4 => Height,
        5 => Folded,
        6 => ParentFrame,
        7 => Text,
        8 => Color,
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
                Width = (uint)value;
                break;
            case 4:
                Height = (uint)value;
                break;
            case 5:
                Folded = (bool)value;
                break;
            case 6:
                ParentFrame = (string)value;
                break;
            case 7:
                Text = (string)value;
                break;
            case 8:
                Color = (Color)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override void AddChild(Element child)
         => throw new NotImplementedException($"Elements of type '{ElementName}' cannot have child elements.");
}