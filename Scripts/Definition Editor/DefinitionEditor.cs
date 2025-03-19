using Godot;
using System.Xml;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Definitions
{
    [GlobalClass]
    public partial class DefinitionEditor : VBoxContainer
    {
        /* Public properties. */
        public LineField Opcode { get; private set; }

        public TabBar TabBar { get; private set; }

        public ParameterBox Parameters { get; private set; }

        public ImplementationBox Implementation { get; private set; }

        public VBoxContainer Metadata { get; private set; }
        public IconInspector Icon { get; private set; }
        public LineField DisplayName { get; private set; }
        public MultilineField Description { get; private set; }
        public LineField Category { get; private set; }

        public VBoxContainer Editor { get; private set; }
        public EditorNodeInfoInspector EditorNodeInfo { get; private set; }
        public MultilineField Preview { get; private set; }

        /* Godot overrides. */
        public override void _Ready()
        {
            AddButtons();

            Opcode = new LineField() { LabelText = "Opcode" };
            AddChild(Opcode);

            TabBar = new();
            TabBar.AddTab("Parameters");
            TabBar.AddTab("Implementation");
            TabBar.AddTab("Metadata");
            TabBar.AddTab("Editor");
            TabBar.AddTab("Pre-Instructions");
            TabBar.AddTab("Post-Instructions");
            AddChild(TabBar);

            ScrollContainer scroll = new();
            scroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.SizeFlagsVertical = SizeFlags.ExpandFill;
            AddChild(scroll);

            Parameters = new();
            scroll.AddChild(Parameters);

            Implementation = new();
            scroll.AddChild(Implementation);

            Metadata = new();
            Metadata.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(Metadata);
            DisplayName = new() { LabelText = "Display Name" };
            Metadata.AddChild(DisplayName);
            Description = new() { LabelText = "Description", Height = 128 };
            Metadata.AddChild(Description);
            Category = new() { LabelText = "Category" };
            Metadata.AddChild(Category);
            Icon = new();
            Metadata.AddChild(Icon);

            Editor = new();
            Editor.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(Editor);
            EditorNodeInfo = new();
            Editor.AddChild(EditorNodeInfo);
            Preview = new();
            Preview.Height = 128;
            Editor.AddChild(Preview);
        }

        public override void _Process(double delta)
        {
            Parameters.Visible = TabBar.CurrentTab == 0;
            Implementation.Visible = TabBar.CurrentTab == 1;
            Metadata.Visible = TabBar.CurrentTab == 2;
            Editor.Visible = TabBar.CurrentTab == 3;
        }

        /* Private methods. */
        private void AddButtons()
        {
            // Create containers.
            ElementHBox buttons = new();
            AddChild(buttons);

            // Add buttons.
            ButtonElement saveButton = new();
            saveButton.ButtonText = "Save";
            saveButton.Pressed += OnSave;
            buttons.Add(saveButton);

            ButtonElement saveAsButton = new();
            saveButton.ButtonText = "Save As";
            saveAsButton.Pressed += OnSave;
            buttons.Add(saveAsButton);

            ButtonElement saveDebugButton = new();
            saveDebugButton.ButtonText = "Paste To Clipboard";
            saveDebugButton.Pressed += OnSave;
            buttons.Add(saveDebugButton);

            ButtonElement loadButton = new();
            loadButton.ButtonText = "Open";
            loadButton.Pressed += OnOpen;
            buttons.Add(loadButton);

            ButtonElement loadDebugButton = new();
            loadDebugButton.ButtonText = "Load From Clipboard";
            loadDebugButton.Pressed += OnOpen;
            buttons.Add(loadDebugButton);

            AddChild(new HSeparatorElement());
        }


        private InstructionDefinitionDescriptor Compile()
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
            descriptor.Preview = Preview.Value;

            // Add pre-instructions.

            // Add post-instructions.

            return descriptor;
        }

        private void Load(InstructionDefinitionDescriptor descriptor)
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
            Preview.Value = descriptor.Preview;

            // Load pre-instructions.

            // Load post-instructions.
        }


        private void OnSave()
        {
            DisplayServer.ClipboardSet(Compile().GetXml());
        }

        private void OnOpen()
        {
            // Get xml from clipboard.
            string xml = DisplayServer.ClipboardGet();

            // Create XML document.
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            // Create definition descriptor.
            InstructionDefinitionDescriptor descriptor = new(xmlDoc);

            // Load.
            Load(descriptor);
        }
    }
}