using Godot;
using Rusty.Cutscenes;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A cutscene graph frame.
    /// </summary>
    public partial class CutsceneGraphFrame : GraphFrame
    {
        /* Public properties. */
        public InstructionSet InstructionSet { get; set; }
    }
}