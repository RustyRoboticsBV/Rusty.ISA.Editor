using System.Collections.Generic;

namespace Rusty.ActionGraph.Compilation;

/// <summary>
/// A compiler unit.
/// </summary>
internal abstract class Unit
{
    /// <summary>
    /// The input port. Can connect to multiple units.
    /// </summary>
    public List<Unit> From { get; } = new();
}