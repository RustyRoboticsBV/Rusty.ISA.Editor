using Godot;
using Rusty.EditorUI;
using System.Xml;

namespace Rusty.ISA.Editor.Definitions
{
    [GlobalClass]
    public partial class DefinitionEditor : ElementVBox
    {
        public LineField Opcode { get; private set; }

        public ListElement Parameters { get; private set; }

        public ImplementationInspector Implementation { get; private set; }

        public LabelFoldout Metadata { get; private set; }
        public IconInspector Icon { get; private set; }
        public LineField DisplayName { get; private set; }
        public LineField Description { get; private set; }
        public LineField Category { get; private set; }

        public EditorNodeInfoInspector EditorNodeInfo { get; private set; }

        protected override void Init()
        {
            // Base vbox init.
            base.Init();

            // Create containers.
            ElementHBox buttons = new();
            Add(buttons);

            Add(new HSeparatorElement() { Name = "Separator" });

            ElementHBox hbox = new();
            Add(hbox);

            ElementVBox left = new();
            hbox.Add(left);

            ElementVBox right = new();
            right.SizeFlagsVertical = SizeFlags.ExpandFill;
            hbox.Add(right);

            // Add button.
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

            // Add opcode field.
            Opcode = new()
            {
                Name = "Opcode",
                LabelText = "Opcode"
            };
            left.Add(Opcode);

            // Add parameter list.
            Parameters = new()
            {
                Template = new ParameterInspector()
                {
                    Name = "Parameter"
                }
            };
            left.Add(Parameters);

            // Add implementation section.
            Implementation = new();
            right.Add(Implementation);

            // Add meta-data fields.
            left.Add(new HSeparatorElement() { Name = "Separator" });

            DisplayName = new()
            {
                Name = "DisplayName",
                LabelText = "Display Name"
            };
            left.Add(DisplayName);

            Description = new()
            {
                Name = "Description",
                LabelText = "Description"
            };
            left.Add(Description);

            Category = new()
            {
                Name = "Category",
                LabelText = "Category"
            };
            left.Add(Category);

            Icon = new()
            {
                Name = "IconPath",
            };
            left.Add(Icon);

            EditorNodeInfo = new()
            {
                Name = "EditorNodeInfo"
            };
            left.Add(EditorNodeInfo);
        }

        private void OnSave()
        {
            InstructionDefinitionDescriptor descriptor = new();

            // Add opcode.
            descriptor.Opcode = Opcode.Value;

            // Add parameters.
            for (int i = 0; i < Parameters.Count; i++)
            {
                ParameterInspector parameter = Parameters[i].GetAt(0) as ParameterInspector;
                descriptor.Parameters.Add(ParameterDescriptor.Create(parameter.Value));
            }

            // Add implementation.
            descriptor.Implementation = Implementation.Value;

            // Add metadata.
            descriptor.IconPath = Icon.FilePath.Value;
            descriptor.DisplayName = DisplayName.Value;
            descriptor.Description = Description.Value;
            descriptor.Category = Category.Value;

            // Add editor node.
            if (EditorNodeInfo.Foldout.IsOpen)
                descriptor.EditorNodeInfo = new(EditorNodeInfo.Value);

            // Add preview terms.

            // Add pre-instructions.

            // Add post-instructions.

            GD.Print(DefinitionSerializer.Serialize(descriptor));
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

            // Load opcode.
            Opcode.Value = descriptor.Opcode;

            // Load parameters.
            foreach (var parameter in descriptor.Parameters)
            {
                Parameters.Add();
                ParameterInspector inspector = Parameters[^1].GetAt(0) as ParameterInspector;
                inspector.Value = parameter.Generate();
            }

            // Load implementation.
            Implementation.Value = descriptor.Implementation;

            // Load metadata.
            DisplayName.Value = descriptor.DisplayName;
            Description.Value = descriptor.Description;
            Category.Value = descriptor.Category;
            Icon.FilePath.Value = descriptor.IconPath;

            // Load editor node.
            if (descriptor.EditorNodeInfo != null)
                EditorNodeInfo.Value = descriptor.EditorNodeInfo.Generate();

            // Load preview terms.

            // Load pre-instructions.

            // Load post-instructions.
        }
    }
}
