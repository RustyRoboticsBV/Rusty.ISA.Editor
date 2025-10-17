using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public class Ledger
{
    /* Public properties. */
    public InstructionSet Set { get; private set; }
    public GraphEdit GraphEdit { get; private set; }
    public InspectorWindow InspectorWindow { get; private set; }
    public List<LedgerItem> Items { get; private set; } = new();

    /* Private properties. */
    private int NextFrameID { get; set; } = 0;
    private BiDict<IGraphElement, LedgerItem> ElementsLookup { get; set; } = new();
    private BiDict<Inspector, LedgerItem> InspectorsLookup { get; set; } = new();

    /* Constructors. */
    public Ledger(InstructionSet set, GraphEdit graphEdit, InspectorWindow inspectorWindow)
    {
        Set = set;
        GraphEdit = graphEdit;
        InspectorWindow = inspectorWindow;

        // Subscribe event handlers.
        GraphEdit.ElementSelected += OnElementSelected;
        GraphEdit.ElementDeselected += OnElementDeselected;
        GraphEdit.ElementDeleted += OnElementDeleted;
    }

    /* Public methods. */
    /// <summary>
    /// Create a new graph element and an associated inspector.
    /// </summary>
    public LedgerItem CreateElement(InstructionDefinition definition, Vector2I position)
    {
        // Create item.
        LedgerItem item = null;
        switch (definition.Opcode)
        {
            case BuiltIn.JointOpcode:
                GraphJoint joint = GraphEdit.SpawnJoint(position.X, position.Y);
                item = new LedgerJoint(Set, joint);
                break;

            case BuiltIn.CommentOpcode:
                GraphComment comment = GraphEdit.SpawnComment(position.X, position.Y);
                item = new LedgerComment(Set, comment);
                break;

            case BuiltIn.FrameOpcode:
                GraphFrame frame = GraphEdit.SpawnFrame(position.X, position.Y);
                frame.ID = NextFrameID;
                NextFrameID++;
                item = new LedgerFrame(Set, frame);
                break;

            default:
                GraphNode node = GraphEdit.SpawnNode(position.X, position.Y);
                item = new LedgerNode(Set, node, definition.Opcode);
                break;
        }

        // Add to the list and lookups.
        Items.Add(item);
        ElementsLookup.Add(item.Element, item);
        InspectorsLookup.Add(item.Inspector, item);
        return item;
    }

    /// <summary>
    /// Connect two elements.
    /// </summary>
    public void ConnectElements(LedgerItem fromItem, int fromPort, LedgerItem toItem)
    {
        GraphEdit.ConnectElements(fromItem.Element, fromPort, toItem.Element);
    }

    /// <summary>
    /// Delete all graph elements and inspectors.
    /// </summary>
    public void Clear()
    {
        GraphEdit.ClearElements();
        Items.Clear();

        NextFrameID = 0;
        ElementsLookup.Clear();
        InspectorsLookup.Clear();
    }

    /* Private methods. */
    private void OnElementSelected(IGraphElement element)
    {
        LedgerItem item = ElementsLookup[element];
        if (item is not LedgerJoint)
            InspectorWindow.Add(item.Inspector);
    }

    private void OnElementDeselected(IGraphElement element)
    {
        LedgerItem item = ElementsLookup[element];
        if (item is not LedgerJoint)
            InspectorWindow.Remove(item.Inspector);
    }

    private void OnElementDeleted(IGraphElement element)
    {
        LedgerItem item = ElementsLookup[element];
        if (InspectorWindow.Contains(item.Inspector))
            InspectorWindow.Remove(item.Inspector);

        Items.Remove(item);
        ElementsLookup.Remove(item);
        InspectorsLookup.Remove(item);
    }
}