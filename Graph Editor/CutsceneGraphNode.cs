using Godot;
using Godot.Collections;
using Rusty.Cutscenes;

namespace Rusty.CutsceneEditor
{
    [GlobalClass]
    public partial class CutsceneGraphNode : GraphNode
    {
        /* Public properties. */
        public InstructionDefinition Definition { get; set; }

        public VBoxContainer InspectorWindow { get; set; }
        //public InstructionInspector NodeInspector { get; private set; }
        public Array<NodeSlotPair> Slots { get; private set; } = new();

        /* Private properties. */
        private Control TitleContainer { get; set; }
        private TextureRect TitleIcon { get; set; }
        private Label TitleLabel { get; set; }
        private Label Preview { get; set; }

        /* Public methods. */
        /// <summary>
        /// Creates the editor for an instruction type in its default state.
        /// </summary>
        public void Populate(InstructionSet instructionSet, InstructionDefinition definition)
        {
            if (definition == null)
            {
                GD.PrintErr("Tried to populate a graph node with a defitinion, but it was equal to null!");
                return;
            }
            else if (definition.EditorNode == null)
            {
                GD.PrintErr($"Tried to populate a graph node with {definition}, but this instruction has no editor node info.");
                return;
            }

            Definition = definition;

            // Find title container.
            TitleContainer = GetChild(0, true) as Control;

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
            CustomMinimumSize = new Vector2(definition.EditorNode.MinWidth, CustomMinimumSize.Y);

            // Create inspector.
            //NodeInspector = new InstructionInspector(32f, 128f, instructionSet);
            //NodeInspector.SetDefinition(definition);

            // Ensure slots.
            EnsureSlots();

            // Create preview.
            Preview = new();
            Preview.AddThemeFontSizeOverride("font_size", 10);
            AddChild(Preview);
            UpdatePreview();
        }

        /// <summary>
        /// Set the state according to a list of instructions.
        /// </summary>
        public void Set(InstructionInstance[] instances)
        {
            if (instances == null)
                return;

            //NodeInspector.Set(instances);

            UpdateContents();
        }

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
            }
        }

        /* Godot overrides. */
        public override void _Ready()
        {
            NodeSelected += AddToInspectorContainer;
            NodeDeselected += RemoveFromInspectorContainer;
        }

        public override void _Process(double delta)
        {
            if (!Selected)
                return;

            UpdateContents();

            if (Input.IsKeyPressed(Key.Delete) && Selected)
            {
                RemoveFromInspectorContainer();
                QueueFree();
            }
        }

        /* Private methods. */
        private void UpdateContents()
        {
            /*if (NodeInspector.LabelField.Value != "")
                Title = "[" + NodeInspector.LabelField.Value + "] " + Definition.DisplayName;
            else
                Title = Definition.DisplayName;*/

            EnsureSlots();
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

        private void AddToInspectorContainer()
        {
            if (Definition == null)
                return;

            // Recolor title text.
            Color color = EditorNodeInfo.SelectedTextColor;
            if (TitleIcon != null)
                TitleIcon.Modulate = color;
            TitleLabel.AddThemeColorOverride("font_color", color);

            // Add to inspector.
            /*if (InspectorWindow != null)
                InspectorWindow.AddChild(NodeInspector);
            else
                GD.PrintErr($"Graph node {Name} did not have a reference to the properties drawer container.");*/
        }

        private void RemoveFromInspectorContainer()
        {
           /* if (Definition == null || NodeInspector == null)
                return;

            // Recolor title text.
            Color color = Definition.EditorNode.TextColor;
            if (TitleIcon != null)
                TitleIcon.Modulate = color;
            TitleLabel.AddThemeColorOverride("font_color", color);

            // Add to inspector.
            if (InspectorWindow != null)
                InspectorWindow.RemoveChild(NodeInspector);
            else
                GD.PrintErr($"Graph node {Name} did not have a reference to the properties drawer container.");*/
        }

        private void EnsureSlots()
        {
            /*if (Definition == null || NodeInspector == null)
                return;

            // Get outputs.
            Array<ParameterInspector> outputs = NodeInspector.GetOutputs();

            // Should we keep the main output?
            bool overrideMain = false;
            foreach (ParameterInspector output in outputs)
            {
                if (output.Definition is OutputParameter outputParam && outputParam.OverrideDefaultOutput)
                {
                    overrideMain = true;
                    break;
                }
            }

            // Temporarily remove preview.
            if (Preview != null)
                RemoveChild(Preview);

            // Set slot count.
            int slotCount = overrideMain ? outputs.Count : outputs.Count + 1;

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
                    CutsceneGraphEdit graph = GetParent() as CutsceneGraphEdit;
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
                if (overrideMain)
                    Slots[i].RightText.Text = outputs[i].Definition.DisplayName;
                else
                {
                    if (i > 0)
                        Slots[i].RightText.Text = outputs[i - 1].Definition.DisplayName;
                    else
                        Slots[i].RightText.Text = "OUT";
                }
            }

            // Restore preview.
            if (Preview != null)
                AddChild(Preview);*/
        }

        private void UpdatePreview()
        {
            /*if (Definition == null || NodeInspector == null)
                return;

            if (Preview == null)
                return;

            string[] terms = NodeInspector.GetPreviewTerms();
            Preview.Text = "";
            foreach (string term in terms)
            {
                if (term != null)
                    Preview.Text += term;
            }*/
        }
    }
}