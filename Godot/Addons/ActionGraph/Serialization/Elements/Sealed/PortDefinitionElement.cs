////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A definition for a port, which corresponds to a parameter.
/// </summary>
internal sealed class PortDefinitionElement : ArgumentDefinitionElement
{
    /* Public properties. */
    public override int AttributeCount => 3;
    
    /// <summary>
    /// The unique identifier of this port definition.
    /// </summary>
    public override string ID { get; set; }
    /// <summary>
    /// The identifier of the associated parameter definition.
    /// </summary>
    public override string Parameter { get; set; }
    /// <summary>
    /// Whether this port's presence removes a node's default port.
    /// </summary>
    public bool RemoveDefaultPort { get; set; }
    
    /* Public methods. */
    public override object GetAttribute(int index)
    {
        switch (index)
        {
            case 0:
                return ID;
            case 1:
                return Parameter;
            case 2:
                return RemoveDefaultPort;
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
                Parameter = (string)value;
                break;
            case 2:
                RemoveDefaultPort = (bool)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to index {index}.");
        }
    }
    
    public override void AddChild(Element child) => throw new NotImplementedException($"Elements of type {GetType().Name} do not allow child elements.");
}