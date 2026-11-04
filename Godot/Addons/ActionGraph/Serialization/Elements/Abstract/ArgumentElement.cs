////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using Godot;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A base class for elements that represent an instruction argument.
/// </summary>
internal abstract class ArgumentElement : Element
{
    /* Public properties. */
    /// <summary>
    /// The identifier of the associated argument definition.
    /// </summary>
    public abstract string Definition { get; set; }
    /// <summary>
    /// The annotation of this argument.
    /// </summary>
    public abstract string Text { get; set; }
    /// <summary>
    /// The color of this argument.
    /// </summary>
    public abstract Color Color { get; set; }
}