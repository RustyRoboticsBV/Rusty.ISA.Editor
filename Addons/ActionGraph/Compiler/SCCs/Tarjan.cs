using System;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Compilation;

/// <summary>
/// A utility class for Tarjan's algorithm for finding strongly-connected components.
/// </summary>
internal static class Tarjan
{
    /* Private types. */
    /// <summary>
    /// The data used during the algorithm.
    /// </summary>
    private class Memory
    {
        public List<SCC> output = new();

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
    public static List<SCC> GetSCCs(Unit[] units)
    {
        Memory memory = new();
        foreach (Unit unit in units)
        {
            if (!memory.indices.ContainsKey(unit))
                StrongConnect(unit, memory);
        }
        return memory.output;
    }

    /* Private methods. */
    private static void StrongConnect(Unit unit, Memory memory)
    {
        // Set up index and initial low-link index.
        memory.indices[unit] = memory.nextIndex;
        memory.lowLink[unit] = memory.nextIndex;
        memory.nextIndex++;

        // Push to stack.
        memory.stack.Push(unit);
        memory.onStack.Add(unit);

        // Examine successors of the unit.
        switch (unit)
        {
            case NodeUnit node:
                foreach (Unit to in node.To)
                {
                    Examine(unit, to, memory);
                }
                break;
            case MonoUnit mono:
                Examine(unit, mono.To, memory);
                break;
        }

        // If the unit was a root node, pop the stack and generate an SCC.
        if (memory.lowLink[unit] == memory.indices[unit])
        {
            SCC scc = new();
            Unit top = null;
            while (memory.stack.Count > 0 && top != unit)
            {
                top = memory.stack.Pop();
                memory.onStack.Remove(top);
                scc.AddUnit(top);
            }
            memory.output.Add(scc);
        }
    }

    private static void Examine(Unit from, Unit to, Memory memory)
    {
        if (to == null)
            return;

        // Case 1: the successor has not yet been visited.
        if (!memory.indices.ContainsKey(to))
        {
            StrongConnect(to, memory);
            memory.lowLink[from] = Math.Min(memory.lowLink[from], memory.lowLink[to]);
        }

        // Case 2: the successor has been visited already.
        else if (memory.onStack.Contains(to))
            memory.lowLink[from] = Math.Min(memory.lowLink[from], memory.indices[to]);
    }
}