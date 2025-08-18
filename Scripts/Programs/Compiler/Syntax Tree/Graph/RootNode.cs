using System;
using System.Security.Cryptography;

namespace Rusty.ISA.Editor;

/// <summary>
/// A compiler root node.
/// </summary>
public class RootNode : Graphs.RootNode
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

    public override SubNode GetChildAt(int index)
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

    public override InputPort CreateInput()
    {
        InputPort output = new();
        AddInput(output);
        return output;
    }

    public override InputPort GetInputAt(int index)
    {
        return base.GetInputAt(index) as InputPort;
    }

    public override OutputPort CreateOutput()
    {
        OutputPort output = new();
        AddOutput(output);
        return output;
    }

    public override OutputPort GetOutputAt(int index)
    {
        return base.GetOutputAt(index) as OutputPort;
    }

    public override SubNode ToChild()
    {
        SubNode child = new();
        child.Data = Data.Copy();
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
    /// Get an argument on the data object.
    /// </summary>
    public string GetArgument(string id)
    {
        return Data?.GetArgument(id);
    }

    public string ComputeChecksum()
    {
        // Create checksum builder.
        MD5 md5 = MD5.Create();

        // Add our data to the checksum builder.
        Data.AddToChecksum(md5);

        // Add the data of our children to the checksum builder (recursively).
        for (int i = 0; i < ChildCount; i++)
        {
            GetChildAt(i).AddToChecksum(md5);
        }

        // Finish and convert to string.
        md5.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        return BitConverter.ToString(md5.Hash).Replace("-", "");
    }
}