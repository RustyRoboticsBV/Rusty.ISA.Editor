////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A base class for elements that represent a graph element with connection ports.
/// </summary>
internal abstract class ProgramUnitElement : Element
{
    /* Public properties. */
    /// <summary>
    /// The x-position of this program unit on the graph.
    /// </summary>
    public abstract int X { get; set; }
    /// <summary>
    /// The y-position of this program unit on the graph.
    /// </summary>
    public abstract int Y { get; set; }
    /// <summary>
    /// The width of this program unit on the graph.
    /// </summary>
    public abstract uint Width { get; set; }
    /// <summary>
    /// The height of this program unit on the graph.
    /// </summary>
    public abstract uint Height { get; set; }
    /// <summary>
    /// Whether this program unit's inspector has been folded.
    /// </summary>
    public abstract bool Folded { get; set; }
    /// <summary>
    /// The identifier of the parent frame.
    /// </summary>
    public abstract string ParentFrame { get; set; }
}