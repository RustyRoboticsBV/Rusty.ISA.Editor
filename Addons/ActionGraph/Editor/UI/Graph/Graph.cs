using Godot;
using System.Collections.Generic;

namespace Rusty.ActionGraph.Graphs;

public partial class Graph : GraphEdit
{
    /* Private properties. */
    private List<Port> Ports { get; } = new();
    private Dictionary<Port, Port> Edges { get; } = new();

    /* Private methods. */
    private void CollectPorts()
    {
        Ports.Clear();
        CollectPorts(this);
    }

    private void CollectPorts(Control parent)
    {
        if (parent is Port port)
            Ports.Add(port);

        foreach (Node child in parent.GetChildren())
        {
            if (child is Control control)
                CollectPorts(control);
        }
    }
}