////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated file. It should not be edited directly.                //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

namespace Rusty.ActionGraph.Serialization;

/// <summary>
///  A base class for elements that represent a graph element with connection ports.
/// </summary>
internal interface IProgramUnitElement
{
    // <summary>
    // The unique identifier of this program unit.
    // </summary>
    public string ID { get; set; }
    // <summary>
    // The x-position of this program unit on the graph.
    // </summary>
    public int X { get; set; }
    // <summary>
    // The y-position of this program unit on the graph.
    // </summary>
    public int Y { get; set; }
    // <summary>
    // The width of this program unit on the graph.
    // </summary>
    public uint Width { get; set; }
    // <summary>
    // The height of this program unit on the graph.
    // </summary>
    public uint Height { get; set; }
    // <summary>
    // Whether this program unit's inspector has been folded.
    // </summary>
    public bool Folded { get; set; }
}