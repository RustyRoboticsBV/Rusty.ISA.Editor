using Godot;
using Rusty.ISA;

namespace Rusty.ISA.ProgramEditor.Compiler
{
    public abstract class GraphEditFrameCompiler
    {
        /* Private properties. */
        private static uint NextFrameID { get; set; }

        /* Public methods. */
        /// <summary>
        /// Reset the ID generator back to the default state.
        /// </summary>
        public static void ResetIDGenerator()
        {
            NextFrameID = 0;
        }

        /// <summary>
        /// Take a graph edit frame, and convert it and its inspector data to a compiler node.
        /// </summary>
        public static CompilerNode Compile(GraphFrame graphFrame)
        {
            InstructionSet set = graphFrame.InstructionSet;
            int x = Mathf.RoundToInt(graphFrame.PositionOffset.X);
            int y = Mathf.RoundToInt(graphFrame.PositionOffset.Y);
            int width = Mathf.RoundToInt(graphFrame.Size.X);
            int height = Mathf.RoundToInt(graphFrame.Size.Y);
            string title = graphFrame.Inspector.TitleText;
            Color color = graphFrame.Inspector.Color;
            ulong id = NextFrameID;

            // Increment next frame ID.
            NextFrameID++;

            // Create FRM node.
            CompilerNode frame = CompilerNodeMaker.GetFrame(set,
                id.ToString(),
                x.ToString(), y.ToString(),
                width.ToString(), height.ToString(),
                title, color
            );

            // Add EOG sub-node.
            frame.AddChild(CompilerNodeMaker.GetEndOfGroup(set));

            // Return finished node.
            return frame;
        }
    }
}