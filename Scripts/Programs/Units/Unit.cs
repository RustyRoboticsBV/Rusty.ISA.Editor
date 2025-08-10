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
        inspector.Changed += OnInspectorChanged;
    }

    /* Public methods. */
    /// <summary>
    /// Compile this unit.
    /// </summary>
    public abstract RootNode Compile();

    /* Private methods. */
    private void OnInspectorChanged()
    {
        Godot.GD.Print("The inspector " + Opcode + " was changed.");
    }
}