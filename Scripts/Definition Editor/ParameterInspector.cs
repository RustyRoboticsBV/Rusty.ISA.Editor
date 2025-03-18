using Godot;
using System;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Definitions
{
    /// <summary>
    /// A generic parameter definition inspector.
    /// </summary>
    public partial class ParameterInspector : Element
    {
        /* Public methods. */
        public LabelFoldout Foldout { get; private set; }

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

        public IntField MinInt { get; private set; }
        public IntField MaxInt { get; private set; }
        public FloatField MinFloat { get; private set; }
        public FloatField MaxFloat { get; private set; }

        public CheckBoxField RemoveDefault { get; private set; }
        public LineField PreviewArgument { get; private set; }

        public Parameter Value
        {
            get
            {
                switch (Type.Options[Type.Value])
                {
                    case "Bool":
                        return new BoolParameter(ID.Value, DisplayName.Value, Description.Value, DefaultBool.Value);
                    case "Int":
                        return new IntParameter(ID.Value, DisplayName.Value, Description.Value, DefaultInt.Value);
                    case "Int Slider":
                        return new IntSliderParameter(ID.Value, DisplayName.Value, Description.Value, DefaultInt.Value,
                            MinInt.Value, MaxInt.Value);
                    case "Float Slider":
                        return new FloatSliderParameter(ID.Value, DisplayName.Value, Description.Value, DefaultFloat.Value,
                            MinFloat.Value, MaxFloat.Value);
                    case "Float":
                        return new FloatParameter(ID.Value, DisplayName.Value, Description.Value, DefaultFloat.Value);
                    case "Char":
                        return new CharParameter(ID.Value, DisplayName.Value, Description.Value, DefaultChar.Value);
                    case "Text":
                        return new TextParameter(ID.Value, DisplayName.Value, Description.Value, DefaultText.Value);
                    case "Multiline":
                        return new MultilineParameter(ID.Value, DisplayName.Value, Description.Value, DefaultMultiline.Value);
                    case "Color":
                        return new ColorParameter(ID.Value, DisplayName.Value, Description.Value, DefaultColor.Value);
                    case "Output":
                        return new OutputParameter(ID.Value, DisplayName.Value, Description.Value, RemoveDefault.Value,
                            PreviewArgument.Value);
                    default:
                        throw new Exception();
                }
            }
            set
            {
                ID.Value = value.ID;
                DisplayName.Value = value.DisplayName;
                Description.Value = value.Description;
                switch (value)
                {
                    case BoolParameter @bool:
                        DefaultBool.Value = @bool.DefaultValue;
                        break;
                    case IntParameter @int:
                        DefaultInt.Value = @int.DefaultValue;
                        break;
                    case IntSliderParameter islider:
                        DefaultInt.Value = islider.DefaultValue;
                        MinInt.Value = islider.MinValue;
                        MaxInt.Value = islider.MaxValue;
                        break;
                    case FloatParameter @float:
                        DefaultFloat.Value = @float.DefaultValue;
                        break;
                    case FloatSliderParameter fslider:
                        DefaultFloat.Value = fslider.DefaultValue;
                        MinFloat.Value = fslider.MinValue;
                        MaxFloat.Value = fslider.MaxValue;
                        break;
                    case CharParameter @char:
                        DefaultChar.Value = @char.DefaultValue;
                        break;
                    case TextParameter text:
                        DefaultText.Value = text.DefaultValue;
                        break;
                    case MultilineParameter multiline:
                        DefaultMultiline.Value = multiline.DefaultValue;
                        break;
                    case ColorParameter color:
                        DefaultColor.Value = color.DefaultValue;
                        break;
                    case OutputParameter output:
                        RemoveDefault.Value = output.RemoveDefaultOutput;
                        PreviewArgument.Value = output.UseArgumentAsPreview;
                        break;
                }
            }
        }

        /* Private properties. */
        private string LastFilePath { get; set; }

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

                MinInt.Value = otherParameter.MinInt.Value;
                MaxInt.Value = otherParameter.MaxInt.Value;
                MinFloat.Value = otherParameter.MinFloat.Value;
                MaxFloat.Value = otherParameter.MaxFloat.Value;

                RemoveDefault.Value = otherParameter.RemoveDefault.Value;
                PreviewArgument.Value = otherParameter.PreviewArgument.Value;

                return true;
            }
            else
                return false;
        }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            Foldout.HeaderText = ID.Value;
            if (Foldout.HeaderText == "")
                Foldout.HeaderText = "(Nameless parameter)";
            else
                Foldout.HeaderText += $" ({Type.Options[Type.Value].ToLower()})";

            DefaultBool.Visible = Type.Value == 0;
            DefaultInt.Visible = Type.Value == 1 || Type.Value == 2;
            MinInt.Visible = Type.Value == 2;
            MaxInt.Visible = Type.Value == 2;
            DefaultFloat.Visible = Type.Value == 3 || Type.Value == 4;
            MinFloat.Visible = Type.Value == 4;
            MaxFloat.Visible = Type.Value == 4;
            DefaultChar.Visible = Type.Value == 5;
            DefaultText.Visible = Type.Value == 6;
            DefaultMultiline.Visible = Type.Value == 7;
            DefaultColor.Visible = Type.Value == 8;
            RemoveDefault.Visible = Type.Value == 9;
            PreviewArgument.Visible = Type.Value == 9;
        }

        /* Protected methods. */
        protected override void Init()
        {
            base.Init();

            Name = "ParameterInspector";

            Foldout = new()
            {
                HeaderText = "Parameter"
            };
            AddChild(Foldout);

            Type = new()
            {
                LabelText = "Type",
                Options = new string[] { "Bool", "Int", "Int Slider", "Float", "Float Slider", "Char", "Text", "Multiline", "Color", "Output" }
            };
            Foldout.Add(Type);

            ID = new()
            {
                LabelText = "ID",
            };
            Foldout.Add(ID);

            DisplayName = new()
            {
                LabelText = "Display Name",
            };
            Foldout.Add(DisplayName);

            Description = new()
            {
                LabelText = "Description",
                Height = 128
            };
            Foldout.Add(Description);

            DefaultBool = new()
            {
                LabelText = "Default Value"
            };
            Foldout.Add(DefaultBool);

            DefaultInt = new()
            {
                LabelText = "Default Value"
            };
            Foldout.Add(DefaultInt);

            DefaultFloat = new()
            {
                LabelText = "Default Value"
            };
            Foldout.Add(DefaultFloat);

            DefaultChar = new()
            {
                LabelText = "Default Value"
            };
            Foldout.Add(DefaultChar);

            DefaultText = new()
            {
                LabelText = "Default Value"
            };
            Foldout.Add(DefaultText);

            DefaultMultiline = new()
            {
                LabelText = "Default Value",
                Height = 128
            };
            Foldout.Add(DefaultMultiline);

            DefaultColor = new()
            {
                LabelText = "Default Value"
            };
            Foldout.Add(DefaultColor);

            MinInt = new()
            {
                LabelText = "Min Value"
            };
            Foldout.Add(MinInt);

            MinFloat = new()
            {
                LabelText = "Min Value"
            };
            Foldout.Add(MinFloat);

            MaxInt = new()
            {
                LabelText = "Max Value"
            };
            Foldout.Add(MaxInt);

            MaxFloat = new()
            {
                LabelText = "Max Value"
            };
            Foldout.Add(MaxFloat);

            RemoveDefault = new()
            {
                LabelText = "Remove Default Output",
                TooltipText = "Whether or not this output should remove a node's default output, if present."
            };
            Foldout.Add(RemoveDefault);

            PreviewArgument = new()
            {
                LabelText = "Preview Argument",
                TooltipText = "The parameter whose value should be used as the output's label in the editor."
            };
            Foldout.Add(PreviewArgument);
        }
    }
}