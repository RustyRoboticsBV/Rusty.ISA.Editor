using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public partial class GraphEdit : Godot.GraphEdit
{
    /* Private properties. */
    private List<GraphNode> Nodes { get; } = new();

    /* Public methods. */
    public void AddElement(GraphElement element)
    {
        switch (element)
        {
            case GraphNode node:
                Nodes.Add(node);
                break;
        }

        AddChild(element);
    }
}