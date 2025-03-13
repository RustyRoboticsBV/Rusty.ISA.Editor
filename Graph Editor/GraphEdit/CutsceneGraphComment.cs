using Godot;
using Rusty.Cutscenes;
using Rusty.CutsceneEditor.Compiler;

namespace Rusty.CutsceneEditor
{
    /// <summary>
    /// A cutscene graph comment node.
    /// </summary>
    [GlobalClass]
    public partial class CutsceneGraphComment : CutsceneGraphNode
    {
        /* Public properties. */
        public new CommentInspector Inspector
        {
            get => base.Inspector as CommentInspector;
            set => base.Inspector = value;
        }

        /* Private properties. */
        private RichTextLabel Label { get; set; }

        /* Constructors. */
        public CutsceneGraphComment(CutsceneGraphEdit graphEdit)
            : base(graphEdit, graphEdit.InstructionSet[BuiltIn.CommentOpcode])
        {
            Color textColor = Definition.EditorNode.TextColor;
            Color bgColor = Definition.EditorNode.MainColor;
            Color selectedTextColor = EditorNodeInfo.SelectedTextColor;
            Color selectedBgColor = EditorNodeInfo.SelectedMainColor;

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
            margin.AddThemeConstantOverride("margin_top", 8);
            margin.AddThemeConstantOverride("margin_left", 16);
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

            while (TitleContainer.GetChildCount(true) > 0)
            {
                TitleContainer.RemoveChild(TitleContainer.GetChild(0, true));
            }
            TitleContainer.Hide();

            // Create inspector.
            Inspector = new(InstructionSet);
        }

        public override void _Process(double delta)
        {
            if (Label != null)
            {
                Label.Size = Vector2.Zero;
                int parameterIndex = Definition.GetParameterIndex(BuiltIn.CommentText);
                Label.Text = Inspector.CommentText;
                Label.AddThemeColorOverride("default_color", Selected ? EditorNodeInfo.SelectedTextColor : Definition.EditorNode.TextColor);
            }
            Size = Vector2.Zero;
            Size += new Vector2(10f, 10f);
        }
    }
}