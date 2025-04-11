using Godot;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Definitions
{
    [GlobalClass]
    public partial class DefinitionInspector : VBoxContainer
    {
        /* Public properties. */
        public LineField Opcode { get; private set; }

        public TabBar TabBar { get; private set; }
        public BorderContainer Border { get; private set; }
        public Label BorderLabel { get; private set; }

        public ParameterBox Parameters { get; private set; }

        public ImplementationBox Implementation { get; private set; }

        public VBoxContainer Metadata { get; private set; }
        public IconInspector Icon { get; private set; }
        public LineField DisplayName { get; private set; }
        public MultilineField Description { get; private set; }
        public LineField Category { get; private set; }

        public VBoxContainer Editor { get; private set; }
        public EditorNodeInfoInspector EditorNodeInfo { get; private set; }

        public TextEdit Preview { get; private set; }

        public CompileRuleBox PreInstructions { get; private set; }
        public CompileRuleBox PostInstructions { get; private set; }

        /* Public methods. */
        public void Clear()
        {
            Load(new());
        }

        public InstructionDefinitionDescriptor Compile()
        {
            InstructionDefinitionDescriptor descriptor = new();

            // Add opcode.
            descriptor.Opcode = Opcode.Value;

            // Add parameters.
            for (int i = 0; i < Parameters.Parameters.Count; i++)
            {
                descriptor.Parameters.Add(Parameters.Parameters[i].Value);
            }

            // Add implementation.
            descriptor.Implementation = Implementation.Value;

            // Add metadata.
            descriptor.IconPath = Icon.FilePath.Value;
            descriptor.DisplayName = DisplayName.Value;
            descriptor.Description = Description.Value;
            descriptor.Category = Category.Value;

            // Add editor data.
            descriptor.EditorNodeInfo = EditorNodeInfo.Value;
            descriptor.Preview = Preview.Text;

            // Add pre-instructions.
            descriptor.PreInstructions.AddRange(PreInstructions.Get());

            // Add post-instructions.
            descriptor.PostInstructions.AddRange(PostInstructions.Get());

            return descriptor;
        }

        public void Load(InstructionDefinitionDescriptor descriptor)
        {
            // Load opcode.
            Opcode.Value = descriptor.Opcode;

            // Load parameters.
            Parameters.Set(descriptor.Parameters);

            // Load implementation.
            Implementation.Value = descriptor.Implementation;

            // Load metadata.
            Icon.FilePath.Value = descriptor.IconPath;
            DisplayName.Value = descriptor.DisplayName;
            Description.Value = descriptor.Description;
            Category.Value = descriptor.Category;

            // Load editor data.
            EditorNodeInfo.Value = descriptor.EditorNodeInfo;
            Preview.Text = descriptor.Preview;

            // Load pre-instructions.
            PreInstructions.Set(descriptor.PreInstructions);

            // Load post-instructions.
            PostInstructions.Set(descriptor.PostInstructions);
        }

        /* Godot overrides. */
        public override void _Ready()
        {
            Opcode = new();
            AddChild(Opcode);
            Opcode.LabelText = "Opcode";

            ScrollContainer scroll = new();
            AddChild(scroll);
            scroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.SizeFlagsVertical = SizeFlags.ExpandFill;

            Border = new();
            scroll.AddChild(Border);
            Border.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            TabBar = new();
            Border.AddToTop(TabBar);
            TabBar.AddTab("Parameters");
            TabBar.AddTab("Implementation");
            TabBar.AddTab("Metadata");
            TabBar.AddTab("Editor Node");
            TabBar.AddTab("Preview");
            TabBar.AddTab("Pre-Instructions");
            TabBar.AddTab("Post-Instructions");
            TabBar.CurrentTab = 2;
            TabBar.ClipTabs = false;


            Parameters = new();
            Border.AddChild(Parameters);


            Implementation = new();
            Border.AddChild(Implementation);


            Metadata = new();
            Border.AddChild(Metadata);
            Metadata.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            DisplayName = new();
            Metadata.AddChild(DisplayName);
            DisplayName.LabelText = "Display Name";

            Description = new();
            Metadata.AddChild(Description);
            Description.LabelText = "Description";
            Description.Height = 128;

            Category = new();
            Metadata.AddChild(Category);
            Category.LabelText = "Category";

            Icon = new();
            Metadata.AddChild(Icon);
            Icon.SizeFlagsHorizontal = SizeFlags.ExpandFill;


            Editor = new();
            Border.AddChild(Editor);
            Editor.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            EditorNodeInfo = new();
            Editor.AddChild(EditorNodeInfo);


            Preview = new();
            Border.AddChild(Preview);
            Preview.CustomMinimumSize = new(0, 128);
            Preview.SizeFlagsHorizontal = SizeFlags.ExpandFill;


            PreInstructions = new();
            Border.AddChild(PreInstructions);


            PostInstructions = new();
            Border.AddChild(PostInstructions);
        }

        public override void _Process(double delta)
        {
            Parameters.Visible = TabBar.CurrentTab == 0;
            Implementation.Visible = TabBar.CurrentTab == 1;
            Metadata.Visible = TabBar.CurrentTab == 2;
            Editor.Visible = TabBar.CurrentTab == 3;
            Preview.Visible = TabBar.CurrentTab == 4;
            PreInstructions.Visible = TabBar.CurrentTab == 5;
            PostInstructions.Visible = TabBar.CurrentTab == 6;
        }
    }
}