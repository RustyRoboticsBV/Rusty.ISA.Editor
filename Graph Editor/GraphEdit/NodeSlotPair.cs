using Godot;
using System.Collections.Generic;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// An input-output combo element.
    /// </summary>
    public partial class NodeSlotPair : HBoxContainer
    {
        /* Public properties. */
        public CutsceneGraphNode Node { get; private set; }
        public List<NodeSlotPair> Inputs { get; set; } = new();
        public NodeSlotPair Output { get; set; }

        public Label LeftText { get; private set; }
        public Label RightText { get; private set; }

        /* Constructors. */
        public NodeSlotPair(CutsceneGraphNode node)
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill;

            Node = node;

            LeftText = new();
            LeftText.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            AddChild(LeftText);

            RightText = new();
            RightText.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            RightText.HorizontalAlignment = HorizontalAlignment.Right;
            AddChild(RightText);
        }

        /* Public methods. */
        public void ConnectOutput(NodeSlotPair target)
        {
            if (Output != null)
                DisconnectOutput();

            Output = target;
            target.Inputs.Add(this);
        }

        public void DisconnectOutput()
        {
            Output.Inputs.Remove(this);
            Output = null;
        }
    }
}