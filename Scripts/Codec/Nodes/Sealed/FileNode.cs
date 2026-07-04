using System.Security.Cryptography;
using System.Text;

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
    public MetaNode Meta { get; set; }
    /// <summary>
    /// The schema child node.
    /// </summary>
    public SchemaNode Schema { get; set; }
    /// <summary>
    /// The graph child node.
    /// </summary>
    public GraphNode Graph { get; set; }

    /* Constructors. */
    public FileNode(MetaNode meta, SchemaNode schema, GraphNode graph)
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
            sb.AppendLine(Meta.Serialize());
        if (Schema != null)
            sb.AppendLine(Schema.Serialize());
        if (Graph != null)
            sb.AppendLine(Graph.Serialize());

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
}