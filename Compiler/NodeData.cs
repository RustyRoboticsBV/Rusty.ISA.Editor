using Rusty.Cutscenes;

namespace Rusty.CutsceneEditor.Compiler
{
    /// <summary>
    /// The data stored in a compiler graph node.
    /// </summary>
    public class NodeData
    {
        public InstructionSet Set { get; set; }
        public InstructionDefinition Definition { get; set; }
        public InstructionInstance Instance { get; set; }

        public NodeData() { }

        public NodeData(InstructionSet set, InstructionDefinition definition, InstructionInstance instance)
        {
            Set = set;
            Definition = definition;
            Instance = instance;
        }

        public override string ToString()
        {
            return Instance.ToString();
        }
    }
}
