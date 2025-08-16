using Rusty.Graphs;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

/// <summary>
/// A reference to an argument on some node.
/// </summary>
public struct ArgumentReference
{
    public INode Node { get; private set; }
    public string ParameterID { get; private set; }

    public ArgumentReference(INode node, string parameterID)
    {
        Node = node;
        ParameterID = parameterID;
    }

    /* Public methods. */
    public override string ToString()
    {
        return ParameterID + "/" + (Node.Data as NodeData).Instance;
    }

    public void SetValue(string value)
    {
        if (Node.Data is NodeData data)
            data.SetArgument(ParameterID, value);
    }

    public string GetValue()
    {
        if (Node.Data is NodeData data)
            return data.GetArgument(ParameterID);
        return "";
    }
}

/// <summary>
/// The output data of a root node.
/// </summary>
public class OutputArguments
{
    /* Public properties. */
    public bool UsesDefaultOutput { get; private set; } = true;
    public List<ArgumentReference> Arguments { get; } = new();

    public int TotalOutputNumber => UsesDefaultOutput ? Arguments.Count + 1 : Arguments.Count;

    /* Constructors. */
    public OutputArguments(RootNode node)
    {
        CollectOutputData(node);
    }

    /* Public methods. */
    public override string ToString()
    {
        string str = "Default: " + UsesDefaultOutput;
        foreach (ArgumentReference argument in Arguments)
        {
            str += "\n" + argument;
        }
        return str;
    }

    public int GetOutputPortIndex(int outputArgumentIndex)
    {
        if (UsesDefaultOutput)
            return outputArgumentIndex + 1;
        else
            return outputArgumentIndex;
    }

    public int GetOutputPortIndex(ArgumentReference outputArgument)
    {
        return GetOutputPortIndex(Arguments.IndexOf(outputArgument));
    }

    /* Private methods. */
    private void CollectOutputData(INode node)
    {
        if (node.Data is NodeData data)
        {
            if (data.Definition.Opcode == BuiltIn.CommentOpcode || data.Definition.Opcode == BuiltIn.FrameOpcode)
                UsesDefaultOutput = false;

            foreach (Parameter parameter in data.Definition.Parameters)
            {
                if (parameter is OutputParameter output)
                {
                    if (output.RemoveDefaultOutput)
                        UsesDefaultOutput = false;
                    Arguments.Add(new(node, parameter.ID));
                }
            }
        }

        for (int i = 0; i < node.ChildCount; i++)
        {
            CollectOutputData(node.GetChildAt(i));
        }
    }
}