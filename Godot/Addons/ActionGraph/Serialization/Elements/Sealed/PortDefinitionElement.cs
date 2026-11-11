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
internal sealed class PortDefinitionElement : Element, IArgumentDefinitionElement
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 12;
    public const string ELEMENT_NAME = "port_definition";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 3;
    /// <summary>
    /// The unique identifier of this port definition.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The identifier of the associated parameter definition.
    /// </summary>
    public string Parameter { get; set; } = "";
    /// <summary>
    /// Whether this port's presence removes a node's default port.
    /// </summary>
    public bool RemoveDefaultPort { get; set; }
    
    public override bool AllowsChildren => false;
    
    /* Public methods. */
    public override int IndexOfAttribute(string name) => name switch
    {
        "id" => 0,
        "parameter" => 1,
        "remove_default_port" => 2,
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.")
    };
    
    
    public override string NameOfAttribute(int index) => index switch
    {
        0 => "id",
        1 => "parameter",
        2 => "remove_default_port",
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override Type TypeOfAttribute(int index) => index switch
    {
        0 => ID.GetType(),
        1 => Parameter.GetType(),
        2 => RemoveDefaultPort.GetType(),
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override object GetAttribute(int index) => index switch
    {
        0 => ID,
        1 => Parameter,
        2 => RemoveDefaultPort,
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
                Parameter = (string)value;
                break;
            case 2:
                RemoveDefaultPort = (bool)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override void AddChild(Element child)
         => throw new NotImplementedException($"Elements of type '{ElementName}' cannot have child elements.");
}