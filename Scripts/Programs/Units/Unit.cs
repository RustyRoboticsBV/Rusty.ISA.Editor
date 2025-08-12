using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A program unit. It consists of a graph element and its associated inspector.
/// It can also be compiled to and decompiled from a compiler node.
/// </summary>
public abstract class Unit
{
    /* Public properties. */
    public InstructionSet Set { get; set; }
    public string Opcode { get; set; }
    public IGraphElement Element { get; private set; }
    public Inspector Inspector { get; private set; }

    /* Constructors. */
    public Unit(InstructionSet set, string opcode, IGraphElement element, Inspector inspector)
    {
        Set = set;
        Opcode = opcode;
        Element = element;
        Inspector = inspector;
        inspector.Changed += OnInspectorChanged;
    }

    /* Public methods. */
    /// <summary>
    /// Compile this unit.
    /// </summary>
    public abstract RootNode Compile();

    /* Protected methods. */
    protected virtual void OnInspectorChanged()
    {
        // Update node outputs.
        OutputData outputs = OutputDataGetter.GetOutputData(Inspector);
        if (Element is GraphNode node)
        {
            List<string> outputLabels = outputs.GetAllOutputs();
            for (int i = 0; i < outputLabels.Count; i++)
            {
                node.SetOutputPort(i, outputLabels[i]);
            }
        }

        // Update previews.
        // TODO
    }
}