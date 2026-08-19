using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph.CodeGen;

/// <summary>
/// A utility class that implements Tarjan's algorithm for finding strongly-connected components.
/// </summary>
internal static class Tarjan
{
    /* Private types. */
    /// <summary>
    /// The data used during the algorithm.
    /// </summary>
    private class Data
    {
        public List<SCC> result = new();

        public Stack<Unit> stack = new();
        public Dictionary<Unit, int> indices = new();
        public Dictionary<Unit, int> lowLink = new();
        public HashSet<Unit> onStack = new();
        public int nextIndex = 0;
    }

    /* Public methods. */
    /// <summary>
    /// Get the strongly-connected components of a graph.
    /// </summary>
    public static List<SCC> GetSCCs(IEnumerable<Unit> units)
    {
        Data data = new();
        foreach (Unit unit in units)
        {
            if (!data.indices.ContainsKey(unit))
                StrongConnect(unit, data);
        }
        return data.result;
    }

    /* Private methods. */
    private static void StrongConnect(Unit unit, Data data)
    {
        // Set up index and initial low-link index.
        data.indices[unit] = data.nextIndex;
        data.lowLink[unit] = data.nextIndex;
        data.nextIndex++;

        // Push to stack.
        data.stack.Push(unit);
        data.onStack.Add(unit);

        // Examine successors of the unit.
        switch (unit)
        {
            case NodeUnit node:
                foreach (Unit to in node.To)
                {
                    Examine(unit, to, data);
                }
                break;
            case GotoUnit gto:
                Examine(unit, gto.To, data);
                break;
        }

        // If the unit was a root node, pop the stack and generate an SCC.
        if (data.lowLink[unit] == data.indices[unit])
        {
            SCC scc = new();
            Unit top;
            do
            {
                top = data.stack.Pop();
                data.onStack.Remove(top);
                scc.AddUnit(top);
            }
            while (top != unit);
            data.result.Add(scc);
        }
    }

    private static void Examine(Unit from, Unit to, Data data)
    {
        if (to == null)
            return;

        // Case 1: the successor has not yet been visited.
        if (!data.indices.ContainsKey(to))
        {
            StrongConnect(to, data);
            data.lowLink[from] = Math.Min(data.lowLink[from], data.lowLink[to]);
        }

        // Case 2: the successor has been visited already.
        else if (data.onStack.Contains(to))
            data.lowLink[from] = Math.Min(data.lowLink[from], data.indices[to]);
    }
}