////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;
using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// An editor sticky note.
/// </summary>
internal sealed class MemoElement : Element
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 24;
    public const string ELEMENT_NAME = "memo";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 9;
    /// <summary>
    /// The unique identifier of this memo.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The x-position of this memo on the graph.
    /// </summary>
    public int X { get; set; }
    /// <summary>
    /// The y-position of this memo on the graph.
    /// </summary>
    public int Y { get; set; }
    /// <summary>
    /// The width of this memo on the graph.
    /// </summary>
    public uint Width { get; set; }
    /// <summary>
    /// The height of this memo on the graph.
    /// </summary>
    public uint Height { get; set; }
    /// <summary>
    /// Whether this memo's inspector has been folded.
    /// </summary>
    public bool Folded { get; set; }
    /// <summary>
    /// The identifier of the parent frame.
    /// </summary>
    public string ParentFrame { get; set; } = "";
    /// <summary>
    /// The commentary text of this memo.
    /// </summary>
    public string Text { get; set; } = "";
    /// <summary>
    /// The text color of this memo.
    /// </summary>
    public Color Color { get; set; }
    
    public override bool AllowsChildren => false;
    
    /* Public methods. */
    public override int IndexOfAttribute(string name)
    {
        switch (name)
        {
            case "id":
                return 0;
            case "x":
                return 1;
            case "y":
                return 2;
            case "width":
                return 3;
            case "height":
                return 4;
            case "folded":
                return 5;
            case "parent_frame":
                return 6;
            case "text":
                return 7;
            case "color":
                return 8;
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
                return "x";
            case 2:
                return "y";
            case 3:
                return "width";
            case 4:
                return "height";
            case 5:
                return "folded";
            case 6:
                return "parent_frame";
            case 7:
                return "text";
            case 8:
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
                return X.GetType();
            case 2:
                return Y.GetType();
            case 3:
                return Width.GetType();
            case 4:
                return Height.GetType();
            case 5:
                return Folded.GetType();
            case 6:
                return ParentFrame.GetType();
            case 7:
                return Text.GetType();
            case 8:
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
                return X;
            case 2:
                return Y;
            case 3:
                return Width;
            case 4:
                return Height;
            case 5:
                return Folded;
            case 6:
                return ParentFrame;
            case 7:
                return Text;
            case 8:
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
    
    public override void AddChild(Element child) => throw new NotImplementedException($"Elements of type {GetType().Name} do not allow child elements.");
}