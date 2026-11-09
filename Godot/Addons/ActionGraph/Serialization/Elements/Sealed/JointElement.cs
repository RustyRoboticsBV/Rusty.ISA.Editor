////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A visual edge joint in the editor.
/// </summary>
internal sealed class JointElement : Element
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 21;
    public const string ELEMENT_NAME = "joint";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 5;
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
            case "size":
                return 3;
            case "parent_frame":
                return 4;
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
                return "size";
            case 4:
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
                return X.GetType();
            case 2:
                return Y.GetType();
            case 3:
                return Size.GetType();
            case 4:
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
                return X;
            case 2:
                return Y;
            case 3:
                return Size;
            case 4:
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
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override void AddChild(Element child) => throw new NotImplementedException($"Elements of type {GetType().Name} do not allow child elements.");
}