using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A compiler graph.
/// </summary>
public class Graph : Graphs.Graph
{
    /* Constructors. */
    public Graph() : base() { }

    public Graph(List<RootNode> nodes) : base()
    {
        foreach (RootNode node in nodes)
        {
            AddNode(node);
        }
    }
}