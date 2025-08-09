namespace Rusty.ISA.Editor;

/// <summary>
/// A program unit. It consists of a graph element and its associated inspector.
/// It can also be compiled to and decompiled from a compiler node.
/// </summary>
public abstract class Unit
{
    /* Public properties. */
    public InstructionSet Set { get; set; }
    public string Opcode { get; set; }
    public IGraphElement Element { get; private set; }
    public Inspector Inspector { get; private set; }

    /* Constructors. */
    public Unit(InstructionSet set, string opcode, IGraphElement element, Inspector inspector)
    {
        Set = set;
        Opcode = opcode;
        Element = element;
        Inspector = inspector;
    }

    /* Public methods. */
    /// <summary>
    /// Compile this unit.
    /// </summary>
    public abstract RootNode Compile();

    /* Protected methods. */
    /// <summary>
    /// Get an argument from the inspector.
    /// </summary>
    protected object GetInspectorArg(string id)
    {
        IGuiElement element = Inspector.GetAt(ElementInspectorFactory.Parameter + id);
        if (element is IField field)
            return field.Value;
        else
            return "";
    }
}