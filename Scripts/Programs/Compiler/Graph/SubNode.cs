using System.Security.Cryptography;

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

    public new SubNode GetChildAt(int index)
    {
        return base.GetChildAt(index) as SubNode;
    }

    public SubNode GetChildWith(string opcode)
    {
        for (int i = 0; i < ChildCount; i++)
        {
            SubNode child = GetChildAt(i);
            if (child.Opcode == opcode)
                return child;
        }
        return null;
    }

    /// <summary>
    /// Set an argument on the data object.
    /// </summary>
    public void SetArgument(string id, object value)
    {
        Data?.SetArgument(id, value);
    }

    /// <summary>
    /// Get an argument on the data object.
    /// </summary>
    public string GetArgument(string id)
    {
        return Data?.GetArgument(id);
    }

    /// <summary>
    /// Feed this data to a MD5 checksum generator.
    /// </summary>
    public void AddToChecksum(MD5 md5)
    {
        // Add out data to the checksum builder.
        Data.AddToChecksum(md5);

        // Add the data of our children to the checksum builder (recursively).
        for (int i = 0; i < ChildCount; i++)
        {
            GetChildAt(i).AddToChecksum(md5);
        }
    }

    public RootNode ToRoot()
    {
        RootNode root = new();
        root.Data = Data.Copy();
        return root;
    }
}