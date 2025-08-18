using Rusty.Graphs;
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

    /* Public methods. */
    public override RootNode GetNodeAt(int index)
    {
        return base.GetNodeAt(index) as RootNode;
    }

    public override RootNode CreateNode()
    {
        // Create a node.
        RootNode node = new();

        // Add the node.
        AddNode(node);

        // Return the result.
        return node;
    }
}