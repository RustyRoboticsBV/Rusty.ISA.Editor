using System.Collections;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// Represents a graph element's input or output slot. Used for tracking connections.
/// </summary>
public struct Port
{
    public static Port None => new(null, -1);

    public IGraphElement Element;
    public int PortIndex;

    public Port(IGraphElement element, int portIndex)
    {
        Element = element;
        PortIndex = portIndex;
    }

    public override int GetHashCode()
    {
        return Element.GetHashCode() * 23 + PortIndex.GetHashCode();
    }
}

/// <summary>
/// A look-up table for a graph element's outgoing edges.
/// </summary>
public class ElementEdges : IEnumerable<Port>
{
    public IGraphElement Element { get; private set; }

    private List<Port> Ports { get; } = new();

    public ElementEdges(IGraphElement element)
    {
        Element = element;
    }

    public void Connect(int from, Port to)
    {
        while (Ports.Count <= from)
        {
            Ports.Add(Port.None);
        }

        Ports[from] = to;
    }

    public void Disconnect(int from)
    {
        if (Ports.Count <= from)
            return;

        Ports[from] = Port.None;
    }

    public Port GetConnectedTo(int from)
    {
        while (Ports.Count <= from)
        {
            Ports.Add(Port.None);
        }

        return Ports[from];
    }

    public IEnumerator<Port> GetEnumerator() => Ports.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// A look-up table for a GraphEdit's edges.
/// </summary>
public class GraphEdges : IEnumerable<ElementEdges>
{
    private Dictionary<IGraphElement, ElementEdges> Edges { get; } = new();

    public void Connect(IGraphElement fromElement, int fromPort, IGraphElement toElement, int toPort)
    {
        Connect(new(fromElement, fromPort), new(toElement, toPort));
    }

    public void Connect(Port from, Port to)
    {
        // Add from element key if it wasn't there yet.
        if (!Edges.ContainsKey(from.Element))
            Edges.Add(from.Element, new(from.Element));

        // Create edge between ports.
        Edges[from.Element].Connect(from.PortIndex, to);
    }

    public void Disconnect(IGraphElement fromElement, int fromPort)
    {
        Disconnect(new(fromElement, fromPort));
    }

    public void Disconnect(Port from)
    {
        // Do nothing if the element wasn't used as a key.
        if (!Edges.ContainsKey(from.Element))
            return;

        // Disconnect the outgoing edge.
        Edges[from.Element].Disconnect(from.PortIndex);
    }

    public void RemoveElement(IGraphElement element)
    {
        if (!Edges.ContainsKey(element))
            Edges.Remove(element);
    }

    public Port GetConnectedTo(Port from)
    {
        if (!Edges.ContainsKey(from.Element))
            return Port.None;

        return Edges[from.Element].GetConnectedTo(from.PortIndex);
    }

    public IEnumerator<ElementEdges> GetEnumerator() => Edges.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}