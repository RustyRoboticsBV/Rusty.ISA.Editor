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
    public override int AttributeCount => 3;
    
    /// <summary>
    /// The unique identifier of this localized value.
    /// </summary>
    public override string ID { get; set; }
    /// <summary>
    /// The identifier of the language definition.
    /// </summary>
    public string Definition { get; set; }
    /// <summary>
    /// The localized value.
    /// </summary>
    public string Value { get; set; }
    
    /* Public methods. */
    public override object GetAttribute(int index)
    {
        switch (index)
        {
            case 0:
                return ID;
            case 1:
                return Definition;
            case 2:
                return Value;
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
                Definition = (string)value;
                break;
            case 2:
                Value = (string)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to index {index}.");
        }
    }
    
    public override void AddChild(Element child) => throw new NotImplementedException($"Elements of type {GetType().Name} do not allow child elements.");
}