using Godot;
using Rusty.EditorUI;
using Rusty.Xml;

namespace Rusty.CutsceneEditor.Definitions
{
    [GlobalClass]
    public partial class DefinitionEditor : ElementVBox
    {
        public LineField Opcode { get; private set; }
        public ListElement Parameters { get; private set; }
        public MultilineField Implementation { get; private set; }

        public LineField DisplayName { get; private set; }
        public LineField Description { get; private set; }
        public LineField Category { get; private set; }
        public LineField IconPath { get; private set; }

        protected override void Init()
        {
            // Base vbox init.
            base.Init();

            // Create containers.
            ElementHBox buttons = new()
            {
                Name = "Buttons"
            };
            Add(buttons);

            Add(new HSeparatorElement() { Name = "Separator" });

            ElementHBox hbox = new()
            {
                Name = "HBox"
            };
            Add(hbox);

            ElementVBox left = new()
            {
                Name = "Left"
            };
            hbox.Add(left);

            ElementVBox right = new()
            {
                Name = "Right"
            };
            right.SizeFlagsVertical = SizeFlags.ExpandFill;
            hbox.Add(right);

            // Add button.
            ButtonElement saveButton = new()
            {
                Name = "SaveButton",
                ButtonText = "Save"
            };
            saveButton.Pressed += OnSave;
            buttons.Add(saveButton);

            ButtonElement saveAsButton = new()
            {
                Name = "SaveAsButton",
                ButtonText = "Save As"
            };
            saveAsButton.Pressed += OnSave;
            buttons.Add(saveAsButton);

            ButtonElement saveDebugButton = new()
            {
                Name = "SaveDebugButton",
                ButtonText = "Paste To Clipboard"
            };
            saveDebugButton.Pressed += OnSave;
            buttons.Add(saveDebugButton);

            ButtonElement loadButton = new()
            {
                Name = "OpenButton",
                ButtonText = "Open"
            };
            loadButton.Pressed += OnOpen;
            buttons.Add(loadButton);

            ButtonElement loadDebugButton = new()
            {
                Name = "OpenDebugButton",
                ButtonText = "Load From Clipboard"
            };
            loadDebugButton.Pressed += OnOpen;
            buttons.Add(loadDebugButton);

            // Add opcode field.
            Opcode = new()
            {
                Name = "Opcode",
                LabelText = "Opcode"
            };
            left.Add(Opcode);

            // Add implementation field.
            Implementation = new()
            {
                Name = "Implementation",
                LabelText = "Implementation",
                Height = 512,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
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

            IconPath = new()
            {
                Name = "IconPath",
                LabelText = "Icon Path"
            };
            left.Add(IconPath);
        }

        private void OnSave()
        {
            Xml.Element root = new("definition", "");
            root.AddChild(new Xml.Element("opcode", Opcode.Value));
            root.AddChild(new Xml.Element("implementation", "\n" + Implementation.Value + "\n"));

            root.AddChild(new Xml.Element("display_name", DisplayName.Value));
            root.AddChild(new Xml.Element("description", Description.Value));
            root.AddChild(new Xml.Element("category", Category.Value));
            root.AddChild(new Xml.Element("icon", IconPath.Value));

            Document document = new("Document", root);

            GD.Print(document.GenerateXml());

            /*string xml = "<definition>";
            xml += $"\n <opcode>{Opcode.Value}</opcode>";
            xml += $"\n <implementation>\n{Implementation.Value}\n </implementation>";

            xml += $"\n\n <!-- Meta-data -->";
            xml += $"\n <display_name>{DisplayName.Value}</display_name>";
            xml += $"\n <description>{Description.Value}</description>";
            xml += $"\n <category>{Category.Value}</category>";
            xml += $"\n <icon>{IconPath.Value}</icon>";
            xml += "\n</definition>";

            System.IO.File.WriteAllText("file.xml", xml);*/
        }

        private void OnOpen() { }
    }
}
