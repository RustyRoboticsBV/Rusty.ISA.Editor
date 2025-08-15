using System.Security.Cryptography;
using System.Text;

namespace Rusty.ISA.Editor;

/// <summary>
/// A compiler sub-node.
/// </summary>
public class SubNode : Graphs.SubNode
{
    /* Public properties. */
    public new NodeData Data
    {
        get => base.Data as NodeData;
        set => base.Data = value;
    }

    public string Opcode => Data.Definition.Opcode;

    /* Public methods. */
    public override SubNode CreateChild()
    {
        SubNode child = new();
        AddChild(child);
        return child;
    }

    /// <summary>
    /// Set an argument on the data object.
    /// </summary>
    public void SetArgument(string id, object value)
    {
        Data?.SetArgument(id, value);
    }

    /// <summary>
    /// Feed this data to a MD5 checksum generator.
    /// </summary>
    public void AddToChecksum(MD5 md5)
    {
        for (int i = 0; i < ChildCount; i++)
        {
            if (GetChildAt(i) is SubNode child)
                child.AddToChecksum(md5);
        }
    }
}