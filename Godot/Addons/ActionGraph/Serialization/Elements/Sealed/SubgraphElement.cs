////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A subgraph instance.
/// </summary>
internal sealed class SubgraphElement : ProgramUnitElement
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 17;
    public const string ELEMENT_NAME = "subgraph";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 8;
    /// <summary>
    /// The unique identifier of this subgraph.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The identifier of the associated subgraph definition.
    /// </summary>
    public string Definition { get; set; } = "";
    /// <summary>
    /// The x-position of the subgraph on the graph.
    /// </summary>
    public override int X { get; set; }
    /// <summary>
    /// The y-position of this subgraph on the graph.
    /// </summary>
    public override int Y { get; set; }
    /// <summary>
    /// The width of this subgraph on the graph.
    /// </summary>
    public override uint Width { get; set; }
    /// <summary>
    /// The height of this subgraph on the graph.
    /// </summary>
    public override uint Height { get; set; }
    /// <summary>
    /// Whether this subgraph's inspector has been folded.
    /// </summary>
    public override bool Folded { get; set; }
    /// <summary>
    /// The identifier of the parent frame.
    /// </summary>
    public override string ParentFrame { get; set; } = "";
    
    public override bool AllowsChildren => false;
    
    /* Public methods. */
    public override int IndexOfAttribute(string name)
    {
        switch (name)
        {
            case "id":
                return 0;
            case "definition":
                return 1;
            case "x":
                return 2;
            case "y":
                return 3;
            case "width":
                return 4;
            case "height":
                return 5;
            case "folded":
                return 6;
            case "parent_frame":
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
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override void AddChild(Element child) => throw new NotImplementedException($"Elements of type {GetType().Name} do not allow child elements.");
}