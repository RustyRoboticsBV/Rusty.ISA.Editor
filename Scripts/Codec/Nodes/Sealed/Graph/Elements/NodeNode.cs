using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

/// <summary>
/// A node node.
/// </summary>
public sealed class NodeNode : ElementNode
{
    /* Constants. */
    public const string TAG = "node";

    /* Public properties. */
    /// <summary>
    /// The ID of the frame.
    /// </summary>
    public string ID { get; set; } = "";
    /// <summary>
    /// The x-position child node.
    /// </summary>
    public XNode X { get; set; }
    /// <summary>
    /// The y-position child node.
    /// </summary>
    public YNode Y { get; set; }
    /// <summary>
    /// The frame member child node.
    /// </summary>
    public MemberNode Member { get; set; }
    /// <summary>
    /// The start point child node.
    /// </summary>
    public StartNode Start { get; set; }
    /// <summary>
    /// The label child node.
    /// </summary>
    public LabelNode Label { get; set; }
    /// <summary>
    /// The inspector child nodes.
    /// </summary>
    public List<InspectorNode> Inspectors { get; set; } = new();

    /* Constructors. */
    public NodeNode(string ID, XNode x, YNode y, MemberNode member, StartNode start, LabelNode label, List<InspectorNode> inspectors)
    {
        this.ID = ID; 
        X = x;
        Y = y;
        Member = member;
        Start = start;
        Label = label;
        Inspectors = inspectors ?? new();
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();

        if (X != null)
            AppendLine(sb, X.Serialize());
        if (Y != null)
            AppendLine(sb, Y.Serialize());
        if (Member != null)
            AppendLine(sb, Member.Serialize());
        if (Start != null)
            AppendLine(sb, Start.Serialize());
        if (Label != null)
            AppendLine(sb, Label.Serialize());

        foreach (var inspector in Inspectors)
        {
            AppendLine(sb, inspector.Serialize());
        }

        return Wrap(sb.ToString(), TAG, ID);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG, ID);
        X?.Hash(hash);
        Y?.Hash(hash);
        Member?.Hash(hash);
        Start?.Hash(hash);
        Label?.Hash(hash);
        foreach (var inspector in Inspectors)
        {
            inspector.Hash(hash);
        }
        EndHash(hash, TAG);
    }

    /// <summary>
    /// Load from an XML node.
    /// </summary>
    public static NodeNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        XNode x = null;
        YNode y = null;
        MemberNode member = null;
        StartNode start = null;
        LabelNode label = null;
        List<InspectorNode> inspectors = new();
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case XNode.TAG:
                    x = XNode.Load(node);
                    break;
                case YNode.TAG:
                    y = YNode.Load(node);
                    break;
                case MemberNode.TAG:
                    member = MemberNode.Load(node);
                    break;
                case StartNode.TAG:
                    start = StartNode.Load(node);
                    break;
                case LabelNode.TAG:
                    label = LabelNode.Load(node);
                    break;
                case FormNode.TAG:
                    inspectors.Add(FormNode.Load(node));
                    break;
                case OptionNode.TAG:
                    inspectors.Add(OptionNode.Load(node));
                    break;
                case ChoiceNode.TAG:
                    inspectors.Add(ChoiceNode.Load(node));
                    break;
                case TupleNode.TAG:
                    inspectors.Add(TupleNode.Load(node));
                    break;
                case ListNode.TAG:
                    inspectors.Add(ListNode.Load(node));
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }
        return new(GetId(xml), x, y, member, start, label, inspectors);
    }

    /// <summary>
    /// Convert to a list of instructions.
    /// </summary>
    public List<Instruction> ToInstructions(SchemaNode schema)
    {
        // Find node definition.
        NodeDefinitionNode definition = null;
        foreach (var item in schema.Nodes.Nodes)
        {
            if (item.ID == ID)
            {
                definition = item;
                break;
            }
        }
        if (definition == null)
            throw MissingDefinitionException(ID);

        // Convert instructions.
        List<Instruction> instructions = new();
        for (int i = 0; i < Inspectors.Count; i++)
        {
            List<Instruction> sub = null;
            switch (Inspectors[i])
            {
                case FormNode form:
                    sub = form.ToInstructions(schema, definition.Inspectors[i] as FormDefinitionNode);
                    break;
                case OptionNode option:
                    sub = option.ToInstructions(schema, definition.Inspectors[i] as OptionDefinitionNode);
                    break;
                case ChoiceNode choice:
                    sub = choice.ToInstructions(schema, definition.Inspectors[i] as ChoiceDefinitionNode);
                    break;
                case TupleNode tuple:
                    sub = tuple.ToInstructions(schema, definition.Inspectors[i] as TupleDefinitionNode);
                    break;
                case ListNode list:
                    sub = list.ToInstructions(schema, definition.Inspectors[i] as ListDefinitionNode);
                    break;
                default:
                    throw BadNodeException(this, Inspectors[i]);
            }
            AppendLists(instructions, sub);
        }

        // Add start & label.
        foreach (Instruction instruction in instructions)
        {
            if (instruction != null)
            {
                if (Start != null)
                    instruction.Start = Start.ID;
                if (Label != null)
                    instruction.Label = Label.ID;
                break;
            }
        }

        return instructions;
    }
}