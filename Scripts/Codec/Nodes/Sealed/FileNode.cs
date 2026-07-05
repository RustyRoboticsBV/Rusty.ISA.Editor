using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Rusty.ISA.Serialization;

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
        StringBuilder sb = new StringBuilder();
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
    /// Convert to a list of instructions.
    /// </summary>
    public List<Instruction> ToInstructions()
    {
        return Graph.ToInstructions(Schema);
    }
}