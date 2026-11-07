////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;
using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A visual edge joint in the editor.
/// </summary>
internal sealed class JointElement : Element
{
    /* Public properties. */
    public const int ELEMENT_INDEX = 16;
    public const string ELEMENT_NAME = "joint";
    
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    public override int AttributeCount => 8;
    
    /// <summary>
    /// The unique identifier of this joint.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The identifier of the associated edge.
    /// </summary>
    public string Edge { get; set; } = "";
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
    /// The annotation of this joint.
    /// </summary>
    public string Text { get; set; } = "";
    /// <summary>
    /// The color of this joint.
    /// </summary>
    public Color Color { get; set; }
    
    /* Public methods. */
    public override int IndexOfAttribute(string name)
    {
        switch (name)
        {
            case "id":
                return 0;
            case "edge":
                return 1;
            case "x":
                return 2;
            case "y":
                return 3;
            case "size":
                return 4;
            case "parent_frame":
                return 5;
            case "text":
                return 6;
            case "color":
                return 7;
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
                return "edge";
            case 2:
                return "x";
            case 3:
                return "y";
            case 4:
                return "size";
            case 5:
                return "parent_frame";
            case 6:
                return "text";
            case 7:
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
                return Edge.GetType();
            case 2:
                return X.GetType();
            case 3:
                return Y.GetType();
            case 4:
                return Size.GetType();
            case 5:
                return ParentFrame.GetType();
            case 6:
                return Text.GetType();
            case 7:
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
                return Edge;
            case 2:
                return X;
            case 3:
                return Y;
            case 4:
                return Size;
            case 5:
                return ParentFrame;
            case 6:
                return Text;
            case 7:
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
                Edge = (string)value;
                break;
            case 2:
                X = (int)value;
                break;
            case 3:
                Y = (int)value;
                break;
            case 4:
                Size = (uint)value;
                break;
            case 5:
                ParentFrame = (string)value;
                break;
            case 6:
                Text = (string)value;
                break;
            case 7:
                Color = (Color)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override void AddChild(Element child) => throw new NotImplementedException($"Elements of type {GetType().Name} do not allow child elements.");
}