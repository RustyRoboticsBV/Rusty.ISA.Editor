////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated file. It should not be edited directly.                //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

namespace Rusty.ActionGraph.Serialization;

/// <summary>
///  A base class for elements that represent an instruction argument.
/// </summary>
internal interface IArgumentElement
{
    // <summary>
    // The unique identifier of this argument.
    // </summary>
    public string ID { get; set; }
    // <summary>
    // The identifier of the associated argument definition.
    // </summary>
    public string Definition { get; set; }
}