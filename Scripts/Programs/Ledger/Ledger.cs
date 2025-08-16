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
    private BiDict<IGraphElement, LedgerItem> ElementsLookup { get; set; } = new();
    private BiDict<Inspector, LedgerItem> InspectorsLookup { get; set; } = new();

    /* Constructors. */
    public Ledger(InstructionSet set, GraphEdit graphEdit, InspectorWindow inspectorWindow)
    {
        Set = set;
        GraphEdit = graphEdit;
        InspectorWindow = inspectorWindow;
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
                item = new LedgerFrame(Set, frame);
                break;

            default:
                GraphNode node = GraphEdit.SpawnNode(position.X, position.Y);
                item = new LedgerNode(Set, node, definition.Opcode);
                break;
        }

        // Subscribe event handlers.
        item.ElementSelected += OnElementSelected;
        item.ElementDeselected += OnElementDeselected;
        item.ElementDeleted += OnElementDeleted;

        // Add to the list and lookups.
        Items.Add(item);
        ElementsLookup.Add(item.Element, item);
        InspectorsLookup.Add(item.Inspector, item);
        return item;
    }

    /// <summary>
    /// Delete all graph elements and inspectors.
    /// </summary>
    public void Clear()
    {
        foreach (LedgerItem item in Items)
        {
            GraphEdit.RemoveElement(item.Element);
            item.Inspector.GetParent()?.RemoveChild(item.Inspector);
        }
        Items.Clear();
        ElementsLookup.Clear();
        InspectorsLookup.Clear();
    }

    /* Private methods. */
    private void OnElementSelected(LedgerItem item)
    {
        if (item is not LedgerJoint)
            InspectorWindow.Add(item.Inspector);
    }

    private void OnElementDeselected(LedgerItem item)
    {
        if (item is not LedgerJoint)
            InspectorWindow.Remove(item.Inspector);
    }

    private void OnElementDeleted(LedgerItem item)
    {
        Items.Remove(item);
        ElementsLookup.Remove(item);
        InspectorsLookup.Remove(item);
    }
}