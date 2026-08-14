using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Compilation;

/// <summary>
/// A strongly-connected component inside of a directed graph of compiler units.
/// </summary>
internal class SCC
{
    /* Public properties. */
    public HashSet<Unit> Units { get; set; } = new();

    /* Public methods. */
    public override string ToString() => GetRepresentativeUnit().ToString();

    /// <summary>
    /// Add a unit to the strongly-connected component.
    /// </summary>
    public void AddUnit(Unit unit) => Units.Add(unit);

    /// <summary>
    /// Get an arbitrary unit from the SCC that represents it.
    /// </summary>
    public Unit GetRepresentativeUnit()
    {
        foreach (Unit unit in Units)
        {
            return unit;
        }
        throw new NullReferenceException("SCC does not contain any units.");
    }

    /// <summary>
    /// Check if any of the units in this strongly-connected component has a connection from a node that is not part of it.
    /// </summary>
    public bool IsStartComponent()
    {
        foreach (Unit unit in Units)
        {
            foreach (Unit input in unit.From)
            {
                if (!Units.Contains(input))
                    return false;
            }
        }
        return true;
    }
}