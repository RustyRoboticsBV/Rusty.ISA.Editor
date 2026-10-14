using System.Collections.Generic;

namespace Rusty.ActionGraph.CodeGen;

/// <summary>
/// A base class for compiler units (the compiler representation for graphs).
/// </summary>
internal abstract class Unit
{
    /// <summary>
    /// The input port. Can connect to multiple units.
    /// </summary>
    public List<Unit> From { get; } = new();
}