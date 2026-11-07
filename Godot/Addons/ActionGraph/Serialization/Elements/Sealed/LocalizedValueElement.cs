////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A field's value when localized to a specific language.
/// </summary>
internal sealed class LocalizedValueElement : Element
{
    /* Public properties. */
    public const int ELEMENT_INDEX = 25;
    public const string ELEMENT_NAME = "localized_value";
    
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    public override int AttributeCount => 3;
    
    /// <summary>
    /// The unique identifier of this localized value.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The identifier of the associated language definition.
    /// </summary>
    public string Language { get; set; } = "";
    /// <summary>
    /// The localized value.
    /// </summary>
    public string Value { get; set; } = "";
    
    /* Public methods. */
    public override int IndexOfAttribute(string name)
    {
        switch (name)
        {
            case "id":
                return 0;
            case "language":
                return 1;
            case "value":
                return 2;
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
                return "language";
            case 2:
                return "value";
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
                return Language.GetType();
            case 2:
                return Value.GetType();
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
                return Language;
            case 2:
                return Value;
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
                Language = (string)value;
                break;
            case 2:
                Value = (string)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override void AddChild(Element child) => throw new NotImplementedException($"Elements of type {GetType().Name} do not allow child elements.");
}