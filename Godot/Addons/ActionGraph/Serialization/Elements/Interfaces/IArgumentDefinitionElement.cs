////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated file. It should not be edited directly.                //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

namespace Rusty.ActionGraph.Serialization;

/// <summary>
///  A base class for elements that represent an instruction argument definition.
/// </summary>
internal interface IArgumentDefinitionElement
{
    // <summary>
    // The unique identifier of this argument definition.
    // </summary>
    public string ID { get; set; }
    // <summary>
    // The identifier of the associated parameter definition.
    // </summary>
    public string Parameter { get; set; }
}