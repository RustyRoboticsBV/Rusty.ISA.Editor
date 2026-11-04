////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;
using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// An editor node.
/// </summary>
internal sealed class NodeElement : Element
{
    /* Public properties. */
    public override int AttributeCount => 11;
    
    /// <summary>
    /// The unique identifier of this node.
    /// </summary>
    public override string ID { get; set; }
    /// <summary>
    /// The identifier of the associated node definition.
    /// </summary>
    public string Definition { get; set; }
    /// <summary>
    /// The x-position of this node on the graph.
    /// </summary>
    public int X { get; set; }
    /// <summary>
    /// The y-position of this node on the graph.
    /// </summary>
    public int Y { get; set; }
    /// <summary>
    /// The width of this node on the graph.
    /// </summary>
    public uint Width { get; set; }
    /// <summary>
    /// The height of this node on the graph.
    /// </summary>
    public uint Height { get; set; }
    /// <summary>
    /// Whether the node has been folded.
    /// </summary>
    public bool Folded { get; set; }
    /// <summary>
    /// The identifier of the parent frame.
    /// </summary>
    public string ParentFrame { get; set; }
    /// <summary>
    /// The annotation of this node.
    /// </summary>
    public string Text { get; set; }
    /// <summary>
    /// The color of this node.
    /// </summary>
    public Color Color { get; set; }
    /// <summary>
    /// The unique start point name, from which runtime execution of the graph can begin.
    /// </summary>
    public string StartPointName { get; set; }
    
    /* Public methods. */
    public override string NameOfAttribute(int index)
    {
        switch (index)
        {
            case 0:
                return "id";
            case 1:
                return "definition";
            case 2:
                return "x";
            case 3:
                return "y";
            case 4:
                return "width";
            case 5:
                return "height";
            case 6:
                return "folded";
            case 7:
                return "parent_frame";
            case 8:
                return "text";
            case 9:
                return "color";
            case 10:
                return "start_point_name";
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
                return X.GetType();
            case 3:
                return Y.GetType();
            case 4:
                return Width.GetType();
            case 5:
                return Height.GetType();
            case 6:
                return Folded.GetType();
            case 7:
                return ParentFrame.GetType();
            case 8:
                return Text.GetType();
            case 9:
                return Color.GetType();
            case 10:
                return StartPointName.GetType();
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override int IndexOfAttribute(string name)
    {
        switch (name)
        {
            case "id":
                return 11;
            case "definition":
                return 11;
            case "x":
                return 11;
            case "y":
                return 11;
            case "width":
                return 11;
            case "height":
                return 11;
            case "folded":
                return 11;
            case "parent_frame":
                return 11;
            case "text":
                return 11;
            case "color":
                return 11;
            case "start_point_name":
                return 11;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.");
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
                return X;
            case 3:
                return Y;
            case 4:
                return Width;
            case 5:
                return Height;
            case 6:
                return Folded;
            case 7:
                return ParentFrame;
            case 8:
                return Text;
            case 9:
                return Color;
            case 10:
                return StartPointName;
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
                X = (int)value;
                break;
            case 3:
                Y = (int)value;
                break;
            case 4:
                Width = (uint)value;
                break;
            case 5:
                Height = (uint)value;
                break;
            case 6:
                Folded = (bool)value;
                break;
            case 7:
                ParentFrame = (string)value;
                break;
            case 8:
                Text = (string)value;
                break;
            case 9:
                Color = (Color)value;
                break;
            case 10:
                StartPointName = (string)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
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