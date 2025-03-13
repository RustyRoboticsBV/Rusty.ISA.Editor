using Rusty.Cutscenes;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// An inspectable CutsceneGraphEdit element.
    /// </summary>
    public interface IGraphElement
    {
        /// <summary>
        /// The graph edit that this element is contained on.
        /// </summary>
        public CutsceneGraphEdit GraphEdit { get; }
        /// <summary>
        /// The instruction set that this element uses.
        /// </summary>
        public InstructionSet InstructionSet { get; }
        /// <summary>
        /// The instruction definition that this element represents.
        /// </summary>
        public InstructionDefinition Definition { get; }
        /// <summary>
        /// The frame that this element is contained in (if any).
        /// </summary>
        public CutsceneGraphFrame Frame { get; }
        /// <summary>
        /// The root inspector of this element.
        /// </summary>
        public Inspector Inspector { get; }
    }


    /// <summary>
    /// An inspectable CutsceneGraphEdit element.
    /// </summary>
    public interface IGraphElement<T> : IGraphElement where T : Inspector
    {
        /// <summary>
        /// The root inspector of this element.
        /// </summary>
        public new T Inspector { get; }
    }
}