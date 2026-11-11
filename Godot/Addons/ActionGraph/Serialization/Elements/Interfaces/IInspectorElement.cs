////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated file. It should not be edited directly.                //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

namespace Rusty.ActionGraph.Serialization;

/// <summary>
///  A base class for elements that represent an editor inspector.
/// </summary>
internal interface IInspectorElement
{
    // <summary>
    // The unique identifier of this inspector.
    // </summary>
    public string ID { get; set; }
    // <summary>
    // The identifier of the associated inspector definition.
    // </summary>
    public string Definition { get; set; }
    // <summary>
    // Whether this inspector has been folded.
    // </summary>
    public bool Folded { get; set; }
}