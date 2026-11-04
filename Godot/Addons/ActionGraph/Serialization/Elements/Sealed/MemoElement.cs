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
    /* Public properties. */
    public override int AttributeCount => 9;
    
    /// <summary>
    /// The unique identifier of this memo.
    /// </summary>
    public override string ID { get; set; }
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
    /// Whether the memo has been folded.
    /// </summary>
    public bool Folded { get; set; }
    /// <summary>
    /// The identifier of the parent frame.
    /// </summary>
    public string ParentFrame { get; set; }
    /// <summary>
    /// The text of this memo.
    /// </summary>
    public string Text { get; set; }
    /// <summary>
    /// The text color of this memo.
    /// </summary>
    public Color Color { get; set; }
    
    /* Public methods. */
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
                throw new ArgumentOutOfRangeException($"No attribute corresponds to index {index}.");
        }
    }
    
    public override void AddChild(Element child) => throw new NotImplementedException($"Elements of type {GetType().Name} do not allow child elements.");
}