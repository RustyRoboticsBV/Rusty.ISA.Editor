////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A base class for elements that represent an instruction argument definition.
/// </summary>
internal abstract class ArgumentDefinitionElement : Element
{
    /* Public properties. */
    /// <summary>
    /// The identifier of the associated parameter definition.
    /// </summary>
    public abstract string Parameter { get; set; }
}