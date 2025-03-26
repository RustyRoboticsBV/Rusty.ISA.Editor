using Godot;
using Godot.Collections;
using System.Collections.Generic;

namespace Rusty.ISA.Editor.Programs
{
    /// <summary>
    /// A graph edit node representing a collection of instructions.
    /// </summary>
    public partial class GraphInstruction : GraphNode
    {
        /* Public properties. */
        public new NodeInstructionInspector Inspector
        {
            get => base.Inspector as NodeInstructionInspector;
            set => base.Inspector = value;
        }
        public Array<NodeSlotPair> Slots { get; private set; } = new();
        
        /* Private properties. */
        private TextureRect TitleIcon { get; set; }
        private Label TitleLabel { get; set; }
        private Label PreviewLabel { get; set; }

        /* Public methods. */
        /// <summary>
        /// Creates the editor for an instruction type in its default state.
        /// </summary>
        public GraphInstruction(ProgramGraphEdit graphEdit, InstructionDefinition definition)
            : base(graphEdit, definition)
        {
            if (definition == null)
            {
                GD.PrintErr("Tried to populate a graph node with a defitinion, but it was equal to null!");
                return;
            }
            else if (definition.EditorNode == null)
            {
                GD.PrintErr($"Tried to populate a graph node with instruction definition '{definition}', but this instruction "
                    + "has no editor node info.");
                return;
            }

            // Set themes.
            StyleBoxFlat titleStyleBox = new();
            titleStyleBox.BgColor = Definition.EditorNode.MainColor;
            titleStyleBox.ContentMarginLeft = 4f;
            AddThemeStyleboxOverride("titlebar", titleStyleBox);

            StyleBoxFlat titleSelectedStyleBox = new();
            titleSelectedStyleBox.BgColor = EditorNodeInfo.SelectedMainColor;
            titleSelectedStyleBox.ContentMarginLeft = 4f;
            AddThemeStyleboxOverride("titlebar_selected", titleSelectedStyleBox);

            TitleLabel = TitleContainer.GetChild(0, true) as Label;
            TitleLabel.AddThemeColorOverride("font_color", Definition.EditorNode.TextColor);

            // Set icon.
            if (Definition.Icon != null)
            {
                TitleIcon = AddIcon(TitleContainer, Definition.Icon, 24);
                TitleIcon.Modulate = Definition.EditorNode.TextColor;
            }

            // Node contents.
            Title = definition.DisplayName;

            TooltipText = definition.Description;
            CustomMinimumSize = new Vector2(definition.EditorNode.MinWidth, definition.EditorNode.MinHeight);

            // Create inspector.
            Inspector = new NodeInstructionInspector(InstructionSet, Definition);

            // Ensure slots.
            EnsureSlots();

            // Create preview.
            PreviewLabel = new();
            if (definition.EditorNode.EnableWordWrap)
            {
                PreviewLabel.AutowrapMode = TextServer.AutowrapMode.Word;
                PreviewLabel.ClipContents = true;
            }
            PreviewLabel.AddThemeFontSizeOverride("font_size", 10);
            AddChild(PreviewLabel);
            ForcePreviewUpdate();
        }

        /// <summary>
        /// Ensure some number of input/output slot pairs.
        /// </summary>
        public void EnsureSlots(int count)
        {
            // Make the number of labels match the target number.
            while (Slots.Count < count)
            {
                NodeSlotPair slotPair = new(this);
                slotPair.RightText.Text = "OUT";
                if (Slots.Count == 0)
                    slotPair.LeftText.Text = "IN";
                Slots.Add(slotPair);
                AddChild(slotPair);
            }

            // Enable slots.
            SetSlotEnabledLeft(0, true);

            for (int i = 0; i < Slots.Count; i++)
            {
                SetSlotEnabledRight(i, true);
                if (Definition.EditorNode.EnableWordWrap)
                {
                    Slots[i].RightText.AutowrapMode = TextServer.AutowrapMode.Word;
                    Slots[i].RightText.ClipContents = true;
                }
            }
        }

        /// <summary>
        /// Force-update a the node's appearance.
        /// </summary>
        public void ForceUpdate()
        {
            UpdateContents();
            ForcePreviewUpdate();

            if (Input.IsKeyPressed(Key.Delete) && IsSelected)
            {
                OnNodeDeselected();
                QueueFree();
            }
        }

        public void ForcePreviewUpdate()
        {
            Inspector.ForcePreviewUpdate();
            PreviewLabel.Text = Inspector.Preview.Evaluate();
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            if (!IsSelected)
                return;

            UpdateContents();
            Size = Vector2.Zero;
        }

        public override void _ExitTree()
        {
            Node inspectorParent = Inspector.GetParent();
            if (inspectorParent != null)
                inspectorParent.RemoveChild(Inspector);
        }

        /* Private methods. */
        private void UpdateContents()
        {
            if (Inspector.LabelName != "")
                Title = $"[{Inspector.LabelName}] {Definition.DisplayName}";
            else
                Title = Definition.DisplayName;

            EnsureSlots();
            if (Inspector.UpdatedPreview)
                UpdatePreview();
        }

        private TextureRect AddIcon(Control container, Texture2D texture, int size)
        {
            TextureRect icon = new();
            icon.Texture = texture;
            icon.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
            icon.StretchMode = TextureRect.StretchModeEnum.KeepAspect;
            icon.MouseFilter = MouseFilterEnum.Ignore;
            icon.CustomMinimumSize = Vector2.One * size;

            MarginContainer iconContainer = new();
            iconContainer.AddThemeConstantOverride("margin_top", 4);
            iconContainer.AddThemeConstantOverride("margin_bottom", 4);
            iconContainer.AddThemeConstantOverride("margin_right", 2);
            iconContainer.MouseFilter = MouseFilterEnum.Ignore;
            iconContainer.AddChild(icon);

            container.AddChild(iconContainer);
            container.MoveChild(iconContainer, 0);

            return icon;
        }

        protected override void OnNodeSelected()
        {
            base.OnNodeSelected();

            if (Definition == null)
                return;

            // Recolor title text.
            Color color = EditorNodeInfo.SelectedTextColor;
            if (TitleIcon != null)
                TitleIcon.Modulate = color;
            TitleLabel.AddThemeColorOverride("font_color", color);
        }

        protected override void OnNodeDeselected()
        {
            base.OnNodeDeselected();

            if (Definition == null || Inspector == null)
                return;

            // Recolor title text.
            Color color = Definition.EditorNode.TextColor;
            if (TitleIcon != null)
                TitleIcon.Modulate = color;
            TitleLabel.AddThemeColorOverride("font_color", color);
        }

        private void EnsureSlots()
        {
            if (Definition == null || Inspector == null)
                return;

            // Get outputs.
            List<ParameterInspector> outputs = Inspector.GetOutputs();

            // Should we keep the main output?
            bool hideMain = false;
            for (int i = 0; i < outputs.Count; i++)
            {
                if ((outputs[i].Definition as OutputParameter).RemoveDefaultOutput)
                    hideMain = true;
            }

            // Temporarily remove preview.
            if (PreviewLabel != null)
                RemoveChild(PreviewLabel);

            // Set slot count.
            int slotCount = hideMain ? outputs.Count : outputs.Count + 1;

            // Always ensure at least one input and output.
            if (slotCount < 1)
                slotCount = 1;

            // Ensure slot(s).
            EnsureSlots(slotCount);

            // Remove slots if necessary.
            while (Slots.Count > slotCount)
            {
                int index = Slots.Count - 1;

                // Disconnect slot.
                if (Slots[index].Output != null)
                {
                    ProgramGraphEdit graph = GetParent() as ProgramGraphEdit;
                    graph.DisconnectNode(Name, index, Slots[index].Output.Name, 0);
                }

                // Remove slot.
                SetSlotEnabledRight(index, false);
                RemoveChild(Slots[index]);
                Slots.RemoveAt(index);
            }

            // Rename slots.
            for (int i = 0; i < Slots.Count; i++)
            {
                int index = hideMain ? i : i - 1;

                if (index < 0)
                    Slots[i].RightText.Text = "OUT";
                else
                    Slots[i].RightText.Text = outputs[index].Preview.Evaluate();
            }

            // Re-add the preview.
            if (PreviewLabel != null)
                AddChild(PreviewLabel);
        }

        private void UpdatePreview()
        {
            if (Definition == null || Inspector == null || PreviewLabel == null)
                return;

            PreviewLabel.Text = Inspector.Preview.Evaluate();
        }
    }
}