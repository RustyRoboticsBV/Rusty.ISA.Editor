using System.Collections;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A single edge from the graph.
/// </summary>
public class Edge
{
    /* Public properties. */
    public IGraphElement FromElement { get; private set; }
    public int FromPortIndex { get; private set; }
    public IGraphElement ToElement { get; private set; }
    public int ToPortIndex { get; private set; }

    /* Constructors. */
    public Edge(IGraphElement fromElement, int fromPortIndex, IGraphElement toElement, int toPortIndex)
    {
        FromElement = fromElement;
        FromPortIndex = fromPortIndex;
        ToElement = toElement;
        ToPortIndex = toPortIndex;
    }

    /* Public methods. */
    public override string ToString()
    {
        return $"{FromElement.Name} ({FromPortIndex}) => {ToElement.Name} ({ToPortIndex})";
    }
}

/// <summary>
/// A look-up table for a graph element's outgoing edges.
/// </summary>
public class ElementEdges : IEnumerable<Edge>
{
    /* Public properties. */
    public IGraphElement FromElement { get; private set; }
    public Dictionary<int, Edge> Edges { get; } = new();

    /* Constructors. */
    public ElementEdges(IGraphElement element)
    {
        FromElement = element;
    }

    /* Public methods. */
    public void Connect(IGraphElement fromElement, int fromPortIndex, IGraphElement toElement, int toPortIndex)
    {
        Edge edge = new(fromElement, fromPortIndex, toElement, toPortIndex);
        if (Edges.ContainsKey(edge.FromPortIndex))
            Disconnect(edge.FromPortIndex);
        Edges.Add(edge.FromPortIndex, edge);
    }

    public void Disconnect(int fromPortIndex)
    {
        if (Edges.ContainsKey(fromPortIndex))
            Edges.Remove(fromPortIndex);
    }

    public Edge Get(int fromPortIndex)
    {
        if (Edges.ContainsKey(fromPortIndex))
            return Edges[fromPortIndex];
        return null;
    }

    public void RemoveTarget(IGraphElement element)
    {
        foreach (var edge in Edges)
        {
            if (edge.Value.ToElement == element)
                Edges.Remove(edge.Key);
        }
    }

    /* Enumerating. */
    public IEnumerator<Edge> GetEnumerator()
    {
        return Edges.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

/// <summary>
/// A look-up table for a GraphEdit's edges.
/// </summary>
public class GraphEdges : IEnumerable<ElementEdges>
{
    /* Public properties. */
    public Dictionary<IGraphElement, ElementEdges> Elements { get; } = new();

    /* Public methods. */
    public void Connect(IGraphElement fromElement, int fromPortIndex, IGraphElement toElement, int toPortIndex)
    {
        if (!Elements.ContainsKey(fromElement))
            Elements.Add(fromElement, new(fromElement));
        Elements[fromElement].Connect(fromElement, fromPortIndex, toElement, toPortIndex);
    }

    public void Disconnect(IGraphElement fromElement, int fromPortIndex)
    {
        if (Elements.ContainsKey(fromElement))
            Elements[fromElement].Disconnect(fromPortIndex);
    }

    public void RemoveElement(IGraphElement element)
    {
        if (Elements.ContainsKey(element))
            Elements.Remove(element);
        foreach (var elementEdges in Elements)
        {
            elementEdges.Value.RemoveTarget(element);
        }
    }

    public Edge GetEdge(IGraphElement fromElement, int fromPortIndex)
    {
        if (!Elements.ContainsKey(fromElement))
            return null;
        return Elements[fromElement].Edges[fromPortIndex];
    }

    public void Clear()
    {
        Elements.Clear();
    }

    /* Enumerating. */
    public IEnumerator<ElementEdges> GetEnumerator()
    {
        return Elements.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}