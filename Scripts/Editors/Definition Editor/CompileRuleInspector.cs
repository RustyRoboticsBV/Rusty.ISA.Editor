using Godot;
using System;
using System.Collections.Generic;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Definitions
{
    /// <summary>
    /// A generic parameter definition inspector.
    /// </summary>
    public partial class CompileRuleInspector : VBoxContainer
    {
        /* Public methods. */
        public OptionField Type { get; private set; }
        public LineField ID { get; private set; }

        public LineField DisplayName { get; private set; }
        public MultilineField Description { get; private set; }
        public CheckBoxField DefaultEnabled { get; private set; }
        public IntField DefaultSelected { get; private set; }
        public LineField AddButtonText { get; private set; }

        public LineField Opcode { get; private set; }
        public CompileRuleBox ChildRules { get; private set; }

        public TextEdit Preview { get; private set; }

        public CompileRuleDescriptor Value
        {
            get
            {
                switch (Types[Type.Value])
                {
                    case XmlKeywords.InstructionRule:
                        return new InstructionRuleDescriptor(ID.Value, DisplayName.Value, Description.Value, Opcode.Value,
                            Preview.Text);
                    case XmlKeywords.OptionRule:
                        return new OptionRuleDescriptor(ID.Value, DisplayName.Value, Description.Value, ChildRules.GetFirst(),
                            DefaultEnabled.Value, Preview.Text);
                    case XmlKeywords.ChoiceRule:
                        return new ChoiceRuleDescriptor(ID.Value, DisplayName.Value, Description.Value, ChildRules.Get(),
                            DefaultSelected.Value, Preview.Text);
                    case XmlKeywords.TupleRule:
                        return new TupleRuleDescriptor(ID.Value, DisplayName.Value, Description.Value, ChildRules.Get(),
                            Preview.Text);
                    case XmlKeywords.ListRule:
                        return new ListRuleDescriptor(ID.Value, DisplayName.Value, Description.Value, ChildRules.GetFirst(),
                            AddButtonText.Value, Preview.Text);
                    default:
                        throw new Exception(Type.Options[Type.Value]);
                }
            }
            set
            {
                ID.Value = value.ID;
                DisplayName.Value = value.DisplayName;
                Description.Value = value.Description;
                Preview.Text = value.Preview;
                if (value is InstructionRuleDescriptor instruction)
                {
                    Type.Value = Types.IndexOf(XmlKeywords.InstructionRule);
                    Opcode.Value = instruction.Opcode;
                    ChildRules.Clear();
                }
                else if (value is OptionRuleDescriptor option)
                {
                    Type.Value = Types.IndexOf(XmlKeywords.OptionRule);
                    DefaultEnabled.Value = option.DefaultEnabled;
                    ChildRules.Set(option.Type);
                }
                else if (value is ChoiceRuleDescriptor choice)
                {
                    Type.Value = Types.IndexOf(XmlKeywords.ChoiceRule);
                    DefaultSelected.Value = choice.DefaultSelected;
                    ChildRules.Set(choice.Choices);
                }
                else if (value is TupleRuleDescriptor tuple)
                {
                    Type.Value = Types.IndexOf(XmlKeywords.TupleRule);
                    ChildRules.Set(tuple.Types);
                }
                else if (value is ListRuleDescriptor list)
                {
                    Type.Value = Types.IndexOf(XmlKeywords.ListRule);
                    AddButtonText.Value = list.AddButtonText;
                    ChildRules.Set(list.Type);
                }
            }
        }

        public List<string> Types => XmlKeywords.CompileRules;
        public List<string> Labels { get; set; } = new(new string[] { "Instruction", "Option", "Choice", "Tuple", "List" });

        /* Private properties. */
        private TabBar TabBar { get; set; }
        private VBoxContainer Children { get; set; }
        private VBoxContainer Metadata { get; set; }

        /* Constructors. */
        public CompileRuleInspector() : base()
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill;

            Type = new();
            AddChild(Type);
            Type.LabelText = "Type";
            Type.Options = Labels.ToArray();

            ID = new();
            AddChild(ID);
            ID.LabelText = "ID";

            BorderContainer border = new();
            AddChild(border);

            TabBar = new();
            border.AddToTop(TabBar);
            TabBar.AddTab("Contents");
            TabBar.AddTab("Metadata");
            TabBar.AddTab("Preview");
            TabBar.ClipTabs = false;


            Children = new();
            border.AddChild(Children);
            Children.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            Opcode = new();
            Children.AddChild(Opcode);
            Opcode.LabelText = "Opcode";

            ChildRules = new();
            Children.AddChild(ChildRules);


            Metadata = new();
            border.AddChild(Metadata);
            Metadata.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            DisplayName = new();
            Metadata.AddChild(DisplayName);
            DisplayName.LabelText = "Display Name";

            Description = new();
            Metadata.AddChild(Description);
            Description.LabelText = "Description";
            Description.Height = 128;

            DefaultEnabled = new();
            Metadata.AddChild(DefaultEnabled);
            DefaultEnabled.LabelText = "Enabled By Default?";

            DefaultSelected = new();
            Metadata.AddChild(DefaultSelected);
            DefaultSelected.LabelText = "Default Selection";

            AddButtonText = new();
            Metadata.AddChild(AddButtonText);
            AddButtonText.LabelText = "Add Button Text?";


            Preview = new();
            border.AddChild(Preview);
            Preview.CustomMinimumSize = new(0, 128);
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            Children.Visible = TabBar.CurrentTab == 0;
            Metadata.Visible = TabBar.CurrentTab == 1;
            Preview.Visible = TabBar.CurrentTab == 2;

            string type = Types[Type.Value];
            Opcode.Visible = type == XmlKeywords.InstructionRule;
            DefaultEnabled.Visible = type == XmlKeywords.OptionRule;
            DefaultSelected.Visible = type == XmlKeywords.ChoiceRule;
            AddButtonText.Visible = type == XmlKeywords.ListRule;
            ChildRules.Visible = type != XmlKeywords.InstructionRule;
            ChildRules.MonoType = type == XmlKeywords.OptionRule || type == XmlKeywords.ListRule;
            ChildRules.MayNotBeEmpty = Type.Value != 0;
        }
    }
}