using Rusty.Graphs;

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