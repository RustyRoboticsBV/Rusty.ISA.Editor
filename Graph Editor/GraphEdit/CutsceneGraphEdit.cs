using Godot;
using System.Collections.Generic;
using Rusty.Cutscenes;
using Rusty.CutsceneEditor.Compiler;
using Array = Godot.Collections.Array;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A graph edit for cutscene nodes.
    /// </summary>
    [GlobalClass]
    public partial class CutsceneGraphEdit : GraphEdit
    {
        /* Public properties. */
        public static int LineHeight => 32;
        
        [Export] public VBoxContainer InspectorWindow { get; set; }
        [Export] public InstructionSet InstructionSet { get; set; }
        
        public List<CutsceneGraphInstruction> Nodes { get; } = new();
        public List<CutsceneGraphComment> Comments { get; } = new();
        public List<CutsceneGraphFrame> Frames { get; } = new();

        /* Private properties. */
        private AddNodePopup AddNodePopup { get; set; }
        private bool HoldCtrl { get; set; }
        private bool MustOpenAddPopup { get; set; }
        private Vector2 AddPopupPosition { get; set; }
        private Vector2 SpawnPosition { get; set; }

        /* Public methods. */
        /// <summary>
        /// Spawn a new instruction node onto the graph, using an instruction definition as a template.
        /// </summary>
        public CutsceneGraphInstruction SpawnNode(InstructionDefinition definition, Vector2 positionOffset)
        {
            CutsceneGraphInstruction node = new(this, definition);
            Nodes.Add(node);
            AddChild(node);

            node.PositionOffset = positionOffset;

            node.Selected += OnSelect;
            node.Deselected += OnDeselect;
            node.Dragged += OnDragged;

            return node;
        }

        /// <summary>
        /// Remove an instruction node from the graph.
        /// </summary>
        public void DeleteNode(CutsceneGraphInstruction node)
        {
            if (Nodes.Contains(node))
            {
                Nodes.Remove(node);

                if (node.GetParent() != null)
                    node.GetParent().RemoveChild(node);
            }
        }

        /// <summary>
        /// Connect an outpot slot of a node on the graph to the input slot of another node.
        /// </summary>
        public void ConnectNode(CutsceneGraphInstruction fromNode, int fromSlot, CutsceneGraphInstruction toNode)
        {
            // Ensure that enough slots are available.
            fromNode.EnsureSlots(fromSlot + 1);
            toNode.EnsureSlots(1);

            // Connect.
            OnConnect(fromNode.Name, fromSlot, toNode.Name, 0);
        }

        /// <summary>
        /// Disconnect an output slot of a node on the graph from the input slot of another node.
        /// </summary>
        public void DisconnectNode(CutsceneGraphInstruction fromNode, int fromSlot, CutsceneGraphInstruction toNode)
        {
            OnDisconnect(fromNode.Name, fromSlot, toNode.Name, 0);
        }

        /// <summary>
        /// Spawn a new comment onto the graph.
        /// </summary>
        public CutsceneGraphComment SpawnComment(Vector2 positionOffset)
        {
            CutsceneGraphComment comment = new(this);
            Comments.Add(comment);
            AddChild(comment);

            comment.PositionOffset = positionOffset;

            comment.Selected += OnSelect;
            comment.Deselected += OnDeselect;
            comment.Dragged += OnDragged;

            return comment;
        }

        /// <summary>
        /// Spawn a new frame onto the graph.
        /// </summary>
        public CutsceneGraphFrame SpawnFrame(Vector2 positionOffset)
        {
            CutsceneGraphFrame frame = new(this, positionOffset);
            Frames.Add(frame);
            AddChild(frame);

            frame.Selected += OnSelect;
            frame.Deselected += OnDeselect;
            frame.Dragged += OnDragged;

            return frame;
        }

        /* Godot overrides. */
        public override void _EnterTree()
        {
            RightDisconnects = true;

            // Create popup.
            AddNodePopup = new(InstructionSet);
            AddChild(AddNodePopup);
            AddNodePopup.Hide();

            // Find nodes.
            foreach (Node child in GetChildren())
            {
                if (child is CutsceneGraphComment comment)
                    Comments.Add(comment);
                else if (child is CutsceneGraphFrame frame)
                    Frames.Add(frame);
                else if (child is CutsceneGraphInstruction node)
                    Nodes.Add(node);
            }

            // Set up events.
            ConnectionRequest += OnConnect;
            DisconnectionRequest += OnDisconnect;
            DeleteNodesRequest += OnDelete;
            FocusExited += OnFocusExited;

            AddNodePopup.SelectedInstruction += OnPopupSelectedInstruction;
        }

        public override void _Process(double delta)
        {
            if (MustOpenAddPopup)
            {
                ShowAddPopup(AddPopupPosition);
                MustOpenAddPopup = false;
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouse eventMouse)
                AddPopupPosition = eventMouse.Position;

            if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.ButtonIndex == MouseButton.Right)
            {
                MustOpenAddPopup = true;
                SpawnPosition = GetMousePosition();
            }

            else if (@event is InputEventKey eventKey)
            {
                if (eventKey.Keycode == Key.Ctrl)
                    HoldCtrl = eventKey.Pressed;
                else if (eventKey.Pressed && eventKey.Keycode == Key.A && HoldCtrl)
                {
                    MustOpenAddPopup = true;
                    SpawnPosition = GetMousePosition();
                }
            }
        }

        /* Private methods. */
        /// <summary>
        /// Show the "add node" popup menu.
        /// </summary>
        private void ShowAddPopup(Vector2 position)
        {
            // Convert to integer positions.
            int x = Mathf.RoundToInt(position.X);
            int y = Mathf.RoundToInt(position.Y);

            // Open popup.
            AddNodePopup.Position = new Vector2I(x, y);
            AddNodePopup.Popup();

            HoldCtrl = false;
        }

        /// <summary>
        /// Return a node on the graph, using its name.
        /// </summary>
        private IGraphElement GetElement(StringName nodeName)
        {
            foreach (Node child in GetChildren())
            {
                if (child.Name == nodeName && child is IGraphElement element)
                    return element;
            }
            return null;
        }

        /// <summary>
        /// Returns the current mouse position as a graph position offset.
        /// </summary>
        private Vector2 GetMousePosition()
        {
            return (GetGlobalMousePosition() - GlobalPosition + ScrollOffset) / Zoom;
        }


        private void OnPopupSelectedInstruction(InstructionDefinition definition)
        {
            if (definition != null)
            {
                if (definition.Opcode == BuiltIn.CommentOpcode)
                    SpawnComment(SpawnPosition);
                else if (definition.Opcode == BuiltIn.FrameOpcode)
                    SpawnFrame(SpawnPosition);
                else
                    SpawnNode(definition, SpawnPosition);
            }
        }

        private void OnConnect(StringName fromNode, long fromPort, StringName toNode, long toPort)
        {
            IGraphElement fromElement = GetElement(fromNode);
            IGraphElement toElement = GetElement(toNode);

            if (fromElement is CutsceneGraphInstruction from && toElement is CutsceneGraphInstruction to)
            {

                NodeSlotPair fromSlot = from.Slots[(int)fromPort];

                // Disconnect if it the output port was already used.
                if (fromSlot.Output != null)
                {
                    StringName previousToNode = fromSlot.Output.Node.Name;
                    OnDisconnect(fromNode, fromPort, previousToNode, toPort);
                }

                // Connect nodes from the perspective of the graph edit.
                Error error = ConnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
                if (error != Error.Ok)
                {
                    GD.PrintErr($"Tried to connect node '{fromNode}' slot #{fromPort} to '{toNode}' slot #{toPort}, but it failed "
                        + $"with error code '{error}'.");
                    return;
                }

                // Connect nodes from the perspective of the node slots.
                NodeSlotPair toSlot = to.Slots[(int)toPort];
                fromSlot.ConnectOutput(toSlot);
            }
        }

        private void OnDisconnect(StringName fromNode, long fromPort, StringName toNode, long toPort)
        {
            IGraphElement fromElement = GetElement(fromNode);
            if (fromElement is CutsceneGraphInstruction from)
            {
                NodeSlotPair slot = from.Slots[(int)fromPort];

                DisconnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
                slot.DisconnectOutput();
            }
        }

        private void OnDelete(Array nodes)
        {
            foreach (Variant obj in nodes)
            {
                // Get the element.
                StringName name = obj.AsStringName();
                IGraphElement element = GetElement(name);

                if (element == null || element.GraphEdit != this)
                    continue;

                // Remove inspector from inspector window.
                Node inspectorParent = element.Inspector.GetParent();
                if (inspectorParent != null)
                    inspectorParent.RemoveChild(element.Inspector);

                // If the element was a node...
                if (element is CutsceneGraphInstruction node)
                {
                    // Disconnect the node from other nodes.
                    for (int i = 0; i < node.Slots.Count; i++)
                    {
                        NodeSlotPair slot = node.Slots[i];

                        // Disconnect inputs.
                        while (slot.Inputs.Count > 0)
                        {
                            NodeSlotPair fromSlot = slot.Inputs[0];
                            CutsceneGraphInstruction fromNode = fromSlot.Node;
                            int fromIndex = fromNode.Slots.IndexOf(fromSlot);
                            OnDisconnect(fromSlot.Node.Name, fromIndex, name, 0);
                        }

                        // Disconnect output.
                        NodeSlotPair toSlot = slot.Output;
                        if (toSlot != null)
                            OnDisconnect(name, i, toSlot.Node.Name, 0);
                    }

                    // Remove from list.
                    Nodes.Remove(node);
                }

                // If the element was a comment...
                else if (element is CutsceneGraphComment comment)
                {
                    Comments.Remove(comment);
                }

                // If the element was a frame...
                else if (element is CutsceneGraphFrame frame)
                {
                    Frames.Remove(frame);
                }

                // Delete the element.
                element.Delete();
            }
        }

        private void OnSelect(IGraphElement element)
        {
            InspectorWindow.AddChild(element.Inspector);
        }

        private void OnDeselect(IGraphElement element)
        {
            InspectorWindow.RemoveChild(element.Inspector);
        }

        private void OnDragged(IGraphElement element)
        {
            CutsceneGraphFrame frame = null;

            // Check if we've just dragged the element into a frame.
            for (int i = 0; i < Frames.Count; i++)
            {
                if (element == Frames[i])
                    continue;

                float x1 = Frames[i].PositionOffset.X;
                float y1 = Frames[i].PositionOffset.Y;
                float x2 = x1 + Frames[i].Size.X;
                float y2 = y1 + Frames[i].Size.Y;

                Vector2 mousePosition = GetMousePosition();

                if (mousePosition.X > x1 && mousePosition.X < x2 && mousePosition.Y > y1 && mousePosition.Y < y2
                    && (frame == null || Frames[i].IsNestedIn(frame)))
                {
                    frame = Frames[i];
                }
            }

            // If the new frame is different from the old one...
            if (element.Frame != frame)
            {
                // Remove from old frame.
                if (element.Frame != null)
                    element.Frame.RemoveElement(element);

                // Add to new frame.
                if (frame != null)
                {
                    for (int i = 0; i < GetChildCount(); i++)
                    {
                        if (GetChild(i) is IGraphElement selected && selected.IsSelected)
                        {
                            frame.AddElement(selected);
                        }
                    }
                }
            }
        }

        private void OnFocusExited()
        {
            HoldCtrl = false;
        }
    }
}