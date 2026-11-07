////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A base class for elements that represent an editor inspector.
/// </summary>
internal abstract class InspectorElement : Element
{
    /* Public properties. */
    /// <summary>
    /// The identifier of the associated inspector definition.
    /// </summary>
    public abstract string Definition { get; set; }
    /// <summary>
    /// Whether this inspector has been folded.
    /// </summary>
    public abstract bool Folded { get; set; }
    /// <summary>
    /// The annotation of this inspector.
    /// </summary>
    public abstract string Text { get; set; }
    /// <summary>
    /// The color of this inspector.
    /// </summary>
    public abstract Color Color { get; set; }
}