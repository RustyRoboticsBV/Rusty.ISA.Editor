using System;
using System.Collections.Generic;
using Godot;
using Rusty.EditorUI;

namespace Rusty.ISA.DefinitionEditor
{
    /// <summary>
    /// A generic parameter definition inspector.
    /// </summary>
    public partial class ParameterInspector : VBoxContainer
    {
        /* Public methods. */
        public OptionField Type { get; private set; }
        public LineField ID { get; private set; }
        public LineField DisplayName { get; private set; }
        public MultilineField Description { get; private set; }

        public CheckBoxField DefaultBool { get; private set; }
        public IntField DefaultInt { get; private set; }
        public FloatField DefaultFloat { get; private set; }
        public CharField DefaultChar { get; private set; }
        public LineField DefaultText { get; private set; }
        public MultilineField DefaultMultiline { get; private set; }
        public ColorField DefaultColor { get; private set; }
        public CheckBoxField RemoveDefault { get; private set; }

        public IntField MinInt { get; private set; }
        public IntField MaxInt { get; private set; }
        public FloatField MinFloat { get; private set; }
        public FloatField MaxFloat { get; private set; }

        public MultilineField Preview { get; private set; }

        public ParameterDescriptor Value
        {
            get
            {
                switch (Types[Type.Value])
                {
                    case XmlKeywords.BoolParameter:
                        return new BoolParameterDescriptor(ID.Value, DisplayName.Value, Description.Value, DefaultBool.Value,
                            Preview.Value);
                    case XmlKeywords.IntParameter:
                        return new IntParameterDescriptor(ID.Value, DisplayName.Value, Description.Value, DefaultInt.Value,
                            Preview.Value);
                    case XmlKeywords.IntSliderParameter:
                        return new IntSliderParameterDescriptor(ID.Value, DisplayName.Value, Description.Value, DefaultInt.Value,
                            MinInt.Value, MaxInt.Value, Preview.Value);
                    case XmlKeywords.FloatParameter:
                        return new FloatParameterDescriptor(ID.Value, DisplayName.Value, Description.Value, DefaultFloat.Value,
                            Preview.Value);
                    case XmlKeywords.FloatSliderParameter:
                        return new FloatSliderParameterDescriptor(ID.Value, DisplayName.Value, Description.Value,
                            DefaultFloat.Value, MinFloat.Value, MaxFloat.Value, Preview.Value);
                    case XmlKeywords.CharParameter:
                        return new CharParameterDescriptor(ID.Value, DisplayName.Value, Description.Value, DefaultChar.Value,
                            Preview.Value);
                    case XmlKeywords.TextlineParameter:
                        return new TextlineParameterDescriptor(ID.Value, DisplayName.Value, Description.Value, DefaultText.Value,
                            Preview.Value);
                    case XmlKeywords.MultilineParameter:
                        return new MultilineParameterDescriptor(ID.Value, DisplayName.Value, Description.Value,
                            DefaultMultiline.Value, Preview.Value);
                    case XmlKeywords.ColorParameter:
                        return new ColorParameterDescriptor(ID.Value, DisplayName.Value, Description.Value, DefaultColor.Value,
                            Preview.Value);
                    case XmlKeywords.OutputParameter:
                        return new OutputParameterDescriptor(ID.Value, DisplayName.Value, Description.Value, RemoveDefault.Value,
                            Preview.Value);
                    default:
                        throw new Exception(Type.Options[Type.Value]);
                }
            }
            set
            {
                ID.Value = value.ID;
                DisplayName.Value = value.DisplayName;
                Description.Value = value.Description;
                switch (value)
                {
                    case BoolParameterDescriptor @bool:
                        Type.Value = Types.IndexOf(XmlKeywords.BoolParameter);
                        DefaultBool.Value = @bool.DefaultValue;
                        break;
                    case IntParameterDescriptor @int:
                        Type.Value = Types.IndexOf(XmlKeywords.IntParameter);
                        DefaultInt.Value = @int.DefaultValue;
                        break;
                    case IntSliderParameterDescriptor islider:
                        Type.Value = Types.IndexOf(XmlKeywords.IntSliderParameter);
                        DefaultInt.Value = islider.DefaultValue;
                        MinInt.Value = islider.MinValue;
                        MaxInt.Value = islider.MaxValue;
                        break;
                    case FloatParameterDescriptor @float:
                        Type.Value = Types.IndexOf(XmlKeywords.FloatParameter);
                        DefaultFloat.Value = @float.DefaultValue;
                        break;
                    case FloatSliderParameterDescriptor fslider:
                        Type.Value = Types.IndexOf(XmlKeywords.FloatSliderParameter);
                        DefaultFloat.Value = fslider.DefaultValue;
                        MinFloat.Value = fslider.MinValue;
                        MaxFloat.Value = fslider.MaxValue;
                        break;
                    case CharParameterDescriptor @char:
                        Type.Value = Types.IndexOf(XmlKeywords.CharParameter);
                        DefaultChar.Value = @char.DefaultValue;
                        break;
                    case TextlineParameterDescriptor text:
                        Type.Value = Types.IndexOf(XmlKeywords.TextlineParameter);
                        DefaultText.Value = text.DefaultValue;
                        break;
                    case MultilineParameterDescriptor multiline:
                        Type.Value = Types.IndexOf(XmlKeywords.MultilineParameter);
                        DefaultMultiline.Value = multiline.DefaultValue;
                        break;
                    case ColorParameterDescriptor color:
                        Type.Value = Types.IndexOf(XmlKeywords.ColorParameter);
                        DefaultColor.Value = color.DefaultValue;
                        break;
                    case OutputParameterDescriptor output:
                        Type.Value = Types.IndexOf(XmlKeywords.OutputParameter);
                        RemoveDefault.Value = output.RemoveDefaultOutput;
                        break;
                }
            }
        }

        public List<string> Types => XmlKeywords.Parameters;
        public List<string> Labels { get; set; } = new(new string[] { "Bool", "Int", "Int Slider", "Float", "Float Slider",
            "Char", "Text Line", "Multiline Text", "Color", "Output" });

        /* Private properties. */
        private TabBar TabBar { get; set; }
        private VBoxContainer Metadata { get; set; }

        /* Constructors. */
        public ParameterInspector()
        {
            Type = new()
            {
                LabelText = "Type",
                Options = Labels.ToArray()
            };
            AddChild(Type);

            ID = new()
            {
                LabelText = "ID",
            };
            AddChild(ID);

            TabBar = new();
            AddChild(TabBar);
            TabBar.AddTab("Metadata");
            TabBar.AddTab("Preview");

            HBoxContainer contents = new();
            AddChild(contents);

            VSeparator separator = new();
            contents.AddChild(separator);

            Metadata = new();
            contents.AddChild(Metadata);
            Metadata.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            DisplayName = new();
            Metadata.AddChild(DisplayName);
            DisplayName.LabelText = "Display Name";

            Description = new();
            Metadata.AddChild(Description);
            Description.LabelText = "Description";
            Description.Height = 128;

            DefaultBool = new();
            Metadata.AddChild(DefaultBool);
            DefaultBool.LabelText = "Default Value";

            DefaultInt = new();
            Metadata.AddChild(DefaultInt);
            DefaultInt.LabelText = "Default Value";

            DefaultFloat = new();
            Metadata.AddChild(DefaultFloat);
            DefaultFloat.LabelText = "Default Value";

            DefaultChar = new();
            Metadata.AddChild(DefaultChar);
            DefaultChar.LabelText = "Default Value";

            DefaultText = new();
            Metadata.AddChild(DefaultText);
            DefaultText.LabelText = "Default Value";

            DefaultMultiline = new();
            Metadata.AddChild(DefaultMultiline);
            DefaultMultiline.LabelText = "Default Value";
            DefaultMultiline.Height = 128;

            DefaultColor = new();
            Metadata.AddChild(DefaultColor);
            DefaultColor.LabelText = "Default Value";

            MinInt = new();
            Metadata.AddChild(MinInt);
            MinInt.LabelText = "Min Value";

            MinFloat = new();
            Metadata.AddChild(MinFloat);
            MinFloat.LabelText = "Min Value";

            MaxInt = new();
            Metadata.AddChild(MaxInt);
            MaxInt.LabelText = "Max Value";

            MaxFloat = new();
            Metadata.AddChild(MaxFloat);
            MaxFloat.LabelText = "Max Value";

            RemoveDefault = new();
            Metadata.AddChild(RemoveDefault);
            RemoveDefault.LabelText = "Remove Default Output";
            RemoveDefault.TooltipText = "Whether or not this output should remove a node's default output, if present.";


            Preview = new();
            contents.AddChild(Preview);
            Preview.LabelText = "Preview";
            Preview.Height = 256;
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            Metadata.Visible = TabBar.CurrentTab == 0;
            Preview.Visible = TabBar.CurrentTab == 1;

            string type = Types[Type.Value];
            DefaultBool.Visible = type == XmlKeywords.BoolParameter;
            DefaultInt.Visible = type == XmlKeywords.IntParameter || type == XmlKeywords.IntSliderParameter;
            MinInt.Visible = type == XmlKeywords.IntSliderParameter;
            MaxInt.Visible = type == XmlKeywords.IntSliderParameter;
            DefaultFloat.Visible = type == XmlKeywords.FloatParameter || type == XmlKeywords.FloatSliderParameter;
            MinFloat.Visible = type == XmlKeywords.FloatSliderParameter;
            MaxFloat.Visible = type == XmlKeywords.FloatSliderParameter;
            DefaultChar.Visible = type == XmlKeywords.CharParameter;
            DefaultText.Visible = type == XmlKeywords.TextlineParameter;
            DefaultMultiline.Visible = type == XmlKeywords.MultilineParameter;
            DefaultColor.Visible = type == XmlKeywords.ColorParameter;
            RemoveDefault.Visible = type == XmlKeywords.OutputParameter;
        }
    }
}