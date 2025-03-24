using System;
using Rusty.ISA;

namespace Rusty.ISA.Editor.Programs.Compiler
{
    /// <summary>
    /// The data stored in a compiler graph node.
    /// </summary>
    public class NodeData : Graphs.NodeData
    {
        /* Public properties. */
        public InstructionSet Set { get; set; }
        public InstructionDefinition Definition { get; set; }
        public InstructionInstance Instance { get; set; }

        /* Constructors. */
        public NodeData() : base() { }

        public NodeData(InstructionSet set, InstructionDefinition definition, InstructionInstance instance) : this()
        {
            Set = set;
            Definition = definition;
            Instance = instance;
        }

        /* Public methods. */
        public override string GetName()
        {
            return Instance.ToString();
        }

        /// <summary>
        /// Get the instruction's opcode.
        /// </summary>
        public string GetOpcode()
        {
            return Definition.Opcode;
        }

        /// <summary>
        /// Get the number of instruction arguments in this data object.
        /// </summary>
        public int GetArgumentCount()
        {
            return Definition.Parameters.Length;
        }

        /// <summary>
        /// Get an argument's value, using its index.
        /// </summary>
        public string GetArgument(int index)
        {
            return Instance.Arguments[index];
        }

        /// <summary>
        /// Get an argument's value, using its parameter name.
        /// </summary>
        public string GetArgument(string name)
        {
            int index = Definition.GetParameterIndex(name);
            if (index < 0)
                throw new Exception($"Could not find parameter '{name}' in instruction '{GetOpcode()}'!");
            return GetArgument(index);
        }

        /// <summary>
        /// Set an argument's value, using its index.
        /// </summary>
        public void SetArgument(int index, string value)
        {
            Instance.Arguments[index] = value;
        }

        /// <summary>
        /// Set an argument's value, using its parameter name.
        /// </summary>
        public void SetArgument(string name, string value)
        {
            int index = Definition.GetParameterIndex(name);
            SetArgument(index, value);
        }
    }
}