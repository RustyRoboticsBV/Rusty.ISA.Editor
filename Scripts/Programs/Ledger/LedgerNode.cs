using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A node-inspector pair.
/// </summary>
public sealed class LedgerNode : LedgerItem
{
    /* Public properties. */
    public new GraphNode Element => base.Element as GraphNode;
    public new NodeInspector Inspector => base.Inspector as NodeInspector;

    /* Constructors. */
    public LedgerNode(InstructionSet set, GraphNode element, string opcode)
        : base(set, element, new NodeInspector(set, opcode))
    {
        InstructionDefinition definition = set[opcode];
        element.TitleIcon = definition.Icon;
        element.TitleText = definition.DisplayName;
        element.TooltipText = definition.Description;
        element.TitleColor = definition.EditorNode.MainColor;

        OnInspectorChanged();
    }

    /* Protected methods. */
    protected override void OnInspectorChanged()
    {
        // Update node outputs.
        OutputData outputs = OutputDataGetter.GetOutputData(Inspector);
        List<string> outputLabels = outputs.GetAllOutputs();
        for (int i = 0; i < outputLabels.Count; i++)
        {
            Element.SetOutputPort(i, outputLabels[i]);
        }

        // TODO: update preview.
    }
}