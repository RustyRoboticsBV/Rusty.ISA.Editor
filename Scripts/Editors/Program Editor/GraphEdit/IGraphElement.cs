using Godot;
using System;
using Rusty.ISA;

namespace Rusty.ISA.ProgramEditor
{
    /// <summary>
    /// An inspectable GraphEdit element.
    /// </summary>
    public interface IGraphElement
    {
        /* Public properties. */
        public Vector2 PositionOffset { get; set; }
        public Vector2 Size { get; set; }
        public bool IsSelected { get; }

        /// <summary>
        /// The graph edit that this element is contained on.
        /// </summary>
        public ProgramGraphEdit GraphEdit { get; }
        /// <summary>
        /// The frame that this element is contained in (if any).
        /// </summary>
        public GraphFrame Frame { get; set; }

        /// <summary>
        /// The instruction set that this element uses.
        /// </summary>
        public InstructionSet InstructionSet { get; }
        /// <summary>
        /// The instruction definition that this element represents.
        /// </summary>
        public InstructionDefinition Definition { get; }

        /// <summary>
        /// The root inspector of this element.
        /// </summary>
        public Inspector Inspector { get; }

        /* Public events. */
        public event Action<IGraphElement> Selected;
        public event Action<IGraphElement> Deselected;
        public event Action<IGraphElement> Dragged;
        public event Action<IGraphElement> Deleted;

        /* Public methods. */
        public Node GetParent();

        /// <summary>
        /// Delete this element from its parent graph edit.
        /// </summary>
        public void Delete();
    }
}