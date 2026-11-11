////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated file. It should not be edited directly.                //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A definition for an option inspector.
/// </summary>
internal sealed class NodeElement : Element, IProgramUnitElement
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 15;
    public const string ELEMENT_NAME = "node";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 8;
    /// <summary>
    /// The unique identifier of this node.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The identifier of the associated node definition.
    /// </summary>
    public string Definition { get; set; } = "";
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
    /// Whether this node's inspector has been folded.
    /// </summary>
    public bool Folded { get; set; }
    /// <summary>
    /// The identifier of the parent frame.
    /// </summary>
    public string ParentFrame { get; set; } = "";
    
    public override bool AllowsChildren => true;
    
    /* Public methods. */
    public override int IndexOfAttribute(string name) => name switch
    {
        "id" => 0,
        "definition" => 1,
        "x" => 2,
        "y" => 3,
        "width" => 4,
        "height" => 5,
        "folded" => 6,
        "parent_frame" => 7,
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.")
    };
    
    
    public override string NameOfAttribute(int index) => index switch
    {
        0 => "id",
        1 => "definition",
        2 => "x",
        3 => "y",
        4 => "width",
        5 => "height",
        6 => "folded",
        7 => "parent_frame",
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override Type TypeOfAttribute(int index) => index switch
    {
        0 => ID.GetType(),
        1 => Definition.GetType(),
        2 => X.GetType(),
        3 => Y.GetType(),
        4 => Width.GetType(),
        5 => Height.GetType(),
        6 => Folded.GetType(),
        7 => ParentFrame.GetType(),
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override object GetAttribute(int index) => index switch
    {
        0 => ID,
        1 => Definition,
        2 => X,
        3 => Y,
        4 => Width,
        5 => Height,
        6 => Folded,
        7 => ParentFrame,
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
                throw new ArgumentOutOfRangeException($"Elements of type '{ElementName}' cannot have children of type '{child.ElementName}'.");
        }
    }
}