using Godot;

namespace Rusty.ISA.Editor;

public static class ElementInspectorFactory
{
    /* Public methods. */
    public static ElementInspector Create(InstructionSet set, InstructionDefinition definition)
    {
        // Create instruction inspector.
        switch (definition.Opcode)
        {
            case BuiltIn.JointOpcode:
                return new JointInspector(set, definition);

            case BuiltIn.CommentOpcode:
                return new CommentInspector(set, definition);

            case BuiltIn.FrameOpcode:
                return new FrameInspector(set, definition);

            default:
                return new NodeInspector(set, definition);
        }
    }
}