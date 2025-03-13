using Godot;
using Rusty.EditorUI;
using Rusty.Cutscenes;
using Rusty.CutsceneEditor.Compiler;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A cutscene graph comment node.
    /// </summary>
    public partial class CutsceneGraphComment : CutsceneGraphNode
    {
        private RichTextLabel Label { get; set; }

        public override void Populate(InstructionDefinition definition)
        {
            base.Populate(definition);

            Color textColor = definition.EditorNode.TextColor;
            Color bgColor = definition.EditorNode.MainColor;
            Color selectedTextColor = EditorNodeInfo.SelectedTextColor;
            Color selectedBgColor = EditorNodeInfo.SelectedMainColor;

            // Remove old contents.
            while (GetChildCount(true) > 1)
            {
                RemoveChild(GetChild(1, true));
            }

            // Remove all slots.
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].LeftText.Text = "";
                Slots[i].RightText.Text = "";
            }
            ClearAllSlots();
            Slots.Clear();

            // Remove title header.
            GetTitlebarHBox().Hide();
            while (GetTitlebarHBox().GetChildCount() > 0)
            {
                GetTitlebarHBox().RemoveChild(GetTitlebarHBox().GetChild(0, true));
            }

            // Add contents.
            Label = new()
            {
                Name = "Text",
                Text = ((MultilineParameter)Definition.Parameters[Definition.GetParameterIndex(BuiltIn.CommentText)]).DefaultValue,
                CustomMinimumSize = new Vector2(200f, 0f),
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                FitContent = true,
                AutowrapMode = TextServer.AutowrapMode.Off,
                MouseFilter = MouseFilterEnum.Pass
            };
            Label.AddThemeColorOverride("default_color", textColor);

            HBoxContainer contents = new()
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            contents.AddChild(Label);

            MarginContainer margin = new();
            margin.AddThemeConstantOverride("margin_left", 16);
            margin.AddThemeConstantOverride("margin_top", 8);
            margin.AddChild(contents);
            AddChild(margin);

            // Add style overrides.
            AddThemeStyleboxOverride("panel", new StyleBoxFlat()
            {
                BgColor = bgColor
            });
            AddThemeStyleboxOverride("titlebar", new StyleBoxFlat()
            {
                BgColor = bgColor
            });
            AddThemeStyleboxOverride("panel_selected", new StyleBoxFlat()
            {
                BgColor = selectedBgColor
            });
            AddThemeStyleboxOverride("titlebar_selected", new StyleBoxFlat()
            {
                BgColor = selectedBgColor
            });

            // Remove begin label field.
            NodeInspector.Label.Hide();
        }

        public override void _Process(double delta)
        {
            if (Label != null)
            {
                Label.Size = Vector2.Zero;
                int parameterIndex = Definition.GetParameterIndex(BuiltIn.CommentText);
                Label.Text = NodeInspector.GetParameterInspector(parameterIndex).ValueObj.ToString();
                Label.AddThemeColorOverride("default_color", Selected ? EditorNodeInfo.SelectedTextColor : Definition.EditorNode.TextColor);
            }
            Size = Vector2.Zero;
            Size += new Vector2(10f, 10f);
        }
    }
}