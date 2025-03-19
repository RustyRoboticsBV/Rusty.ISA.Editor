using System;
using System.Collections.Generic;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Definitions
{
    /// <summary>
    /// A generic parameter definition inspector.
    /// </summary>
    public partial class ParameterInspector : ElementVBox
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

        /* Constructors. */
        public ParameterInspector() : base() { }

        public ParameterInspector(ParameterInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new ParameterInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is ParameterInspector otherParameter)
            {
                Type.Value = otherParameter.Type.Value;

                ID.Value = otherParameter.ID.Value;
                DisplayName.Value = otherParameter.DisplayName.Value;
                Description.Value = otherParameter.Description.Value;

                DefaultBool.Value = otherParameter.DefaultBool.Value;
                DefaultInt.Value = otherParameter.DefaultInt.Value;
                DefaultFloat.Value = otherParameter.DefaultFloat.Value;
                DefaultChar.Value = otherParameter.DefaultChar.Value;
                DefaultText.Value = otherParameter.DefaultText.Value;
                DefaultMultiline.Value = otherParameter.DefaultMultiline.Value;
                DefaultColor.Value = otherParameter.DefaultColor.Value;
                RemoveDefault.Value = otherParameter.RemoveDefault.Value;

                MinInt.Value = otherParameter.MinInt.Value;
                MaxInt.Value = otherParameter.MaxInt.Value;
                MinFloat.Value = otherParameter.MinFloat.Value;
                MaxFloat.Value = otherParameter.MaxFloat.Value;

                return true;
            }
            else
                return false;
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
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

        /* Protected methods. */
        protected override void Init()
        {
            base.Init();

            Name = "ParameterInspector";

            Type = new()
            {
                LabelText = "Type",
                Options = Labels.ToArray()
            };
            Add(Type);

            ID = new()
            {
                LabelText = "ID",
            };
            Add(ID);

            DisplayName = new()
            {
                LabelText = "Display Name",
            };
            Add(DisplayName);

            Description = new()
            {
                LabelText = "Description",
                Height = 128
            };
            Add(Description);

            DefaultBool = new()
            {
                LabelText = "Default Value"
            };
            Add(DefaultBool);

            DefaultInt = new()
            {
                LabelText = "Default Value"
            };
            Add(DefaultInt);

            DefaultFloat = new()
            {
                LabelText = "Default Value"
            };
            Add(DefaultFloat);

            DefaultChar = new()
            {
                LabelText = "Default Value"
            };
            Add(DefaultChar);

            DefaultText = new()
            {
                LabelText = "Default Value"
            };
            Add(DefaultText);

            DefaultMultiline = new()
            {
                LabelText = "Default Value",
                Height = 128
            };
            Add(DefaultMultiline);

            DefaultColor = new()
            {
                LabelText = "Default Value"
            };
            Add(DefaultColor);

            MinInt = new()
            {
                LabelText = "Min Value"
            };
            Add(MinInt);

            MinFloat = new()
            {
                LabelText = "Min Value"
            };
            Add(MinFloat);

            MaxInt = new()
            {
                LabelText = "Max Value"
            };
            Add(MaxInt);

            MaxFloat = new()
            {
                LabelText = "Max Value"
            };
            Add(MaxFloat);

            RemoveDefault = new()
            {
                LabelText = "Remove Default Output",
                TooltipText = "Whether or not this output should remove a node's default output, if present."
            };
            Add(RemoveDefault);

            Preview = new()
            {
                LabelText = "Preview",
                Height = 128
            };
            Add(Preview);
        }
    }
}