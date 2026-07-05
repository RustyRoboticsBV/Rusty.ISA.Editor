using Rusty.ActionGraph.Runtime;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Dictionary = Godot.Collections.Dictionary<string, string>;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A file node.
/// </summary>
public sealed class FileNode : ElementNode
{
    /* Constants. */
    public const string TAG = "file";

    /* Public properties. */
    /// <summary>
    /// The metadata child node.
    /// </summary>
    public MetadataNode Meta { get; set; }
    /// <summary>
    /// The schema child node.
    /// </summary>
    public SchemaNode Schema { get; set; }
    /// <summary>
    /// The graph child node.
    /// </summary>
    public GraphNode Graph { get; set; }

    /* Constructors. */
    public FileNode(MetadataNode meta, SchemaNode schema, GraphNode graph)
    {
        Meta = meta;
        Schema = schema;
        Graph = graph;
    }

    /* Public methods. */
    public override string Serialize()
    {
        StringBuilder sb = new();
        if (Meta != null)
            AppendLine(sb, Meta.Serialize());
        if (Schema != null)
            AppendLine(sb, Schema.Serialize());
        if (Graph != null)
            AppendLine(sb, Graph.Serialize());

        return Wrap(sb.ToString(), TAG);
    }

    public override void Hash(HashAlgorithm hash)
    {
        StartHash(hash, TAG);
        Meta?.Hash(hash);
        Schema?.Hash(hash);
        Graph?.Hash(hash);
        EndHash(hash, TAG);
    }

    public static FileNode Load(XmlNode xml)
    {
        CheckTagMismatch(xml, TAG);

        MetadataNode meta = null;
        SchemaNode schema = null;
        GraphNode graph = null;
        foreach (XmlNode node in xml.ChildNodes)
        {
            switch (node.Name)
            {
                case MetadataNode.TAG:
                    meta = MetadataNode.Load(node);
                    break;
                case SchemaNode.TAG:
                    schema = SchemaNode.Load(node);
                    break;
                case GraphNode.TAG:
                    graph = GraphNode.Load(node);
                    break;
                default:
                    throw InvalidChildException(node, TAG);
            }
        }

        return new(meta, schema, graph);
    }

    /// <summary>
    /// Convert to an instruction set.
    /// </summary>
    /// <returns></returns>
    public InstructionSet ToInstructionSet()
    {
        if (Schema?.Instructions?.Instructions != null)
        {
            List<InstructionDefinition> definitions = new();
            foreach (InstructionDefinitionNode instruction in Schema.Instructions.Instructions)
            {
                List<string> parameters = new();
                foreach (ParameterDefinitionNode parameter in instruction.Parameters)
                {
                    parameters.Add(parameter.ID);
                }
                definitions.Add(new(instruction.Opcode, parameters.ToArray(), instruction.ExecutionHandler.Name));
            }
            return new(definitions.ToArray());
        }
        else
            return new();
    }

    /// <summary>
    /// Convert to a list of instructions.
    /// </summary>
    public List<Instruction> ToInstructions()
    {
        return Graph.ToInstructions(Schema);
    }

    /// <summary>
    /// Convert to a InstructionProgram.
    /// </summary>
    public InstructionProgram ToProgram()
    {
        // Load metadata.
        Dictionary metadata = new();
        if (Meta?.Fields != null)
        {
            foreach (var field in Meta.Fields)
            {
                metadata.Add(field.ID, field.Value);
            }
        }

        // Create program.
        InstructionProgram program = new(metadata, ToInstructionSet(), ToInstructions().ToArray());

        // Set resource name.
        if (metadata.TryGetValue("title", out string title))
            program.ResourceName = title;
        else if (metadata.TryGetValue("name", out string name))
            program.ResourceName = name;

        return program;
    }
}