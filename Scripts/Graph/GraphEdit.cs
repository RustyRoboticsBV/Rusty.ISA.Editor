using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public partial class GraphEdit : Godot.GraphEdit
{
    /* Private properties. */
    private List<GraphNode> Nodes { get; } = new();
    private List<GraphComment> Comments { get; } = new();
    private List<GraphFrame> Frames { get; } = new();

    /* Public methods. */
    public void AddElement(GraphElement element)
    {
        switch (element)
        {
            case GraphNode node:
                Nodes.Add(node);
                break;
            case GraphComment comment:
                Comments.Add(comment);
                break;
            case GraphFrame frame:
                Frames.Add(frame);
                break;
        }

        AddChild(element);
    }
}