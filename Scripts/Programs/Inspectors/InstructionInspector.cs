using Godot;
using System.Collections.Generic;

namespace Rusty.ISA.Editor;

public partial class InstructionInspector : Element
{
    /* Public properties. */
    public Dictionary<string, Field> Parameters { get; } = new();

    /* Private properties. */
    private BorderContainer Border { get; set; }

    /* Constructors. */
    public InstructionInspector()
    {
        Border = new();
        AddChild(Border);
        Border.SizeFlagsHorizontal = SizeFlags.ExpandFill;
    }

    public InstructionInspector(InstructionDefinition definition) : this()
    {
        // Add parameter fields.
        for (int i = 0; i < definition.Parameters.Length; i++)
        {
            Field field = CreateParameterField(definition.Parameters[i]);
            if (field != null)
            {
                Parameters.Add(definition.Parameters[i].ID, field);
                Border.AddToContents(field as Control);
            }
        }
    }

    /* Public methods. */
    public override Element Copy()
    {
        InstructionInspector inspector = new();
        inspector.CopyFrom(this);
        return inspector;
    }

    public void CopyFrom(InstructionInspector other)
    {
        base.CopyFrom(other);

        foreach (var parameter in other.Parameters)
        {
            //Parameters.Add(parameter.Key, parameter.Value.Copy());
        }
    }

    /* Private methods. */
    private Field CreateParameterField(Parameter parameter)
    {
        switch (parameter)
        {
            case BoolParameter @bool:
                return new BoolField()
                {
                    LabelText = parameter.DisplayName,
                    Value = @bool.DefaultValue
                };
            case IntParameter @int:
                return new IntField()
                {
                    LabelText = parameter.DisplayName,
                    Value = @int.DefaultValue
                };
            case IntSliderParameter islider:
                return new IntSliderField()
                {
                    LabelText = parameter.DisplayName,
                    Value = islider.DefaultValue,
                    MinValue = islider.MinValue,
                    MaxValue = islider.MaxValue
                };
            case FloatParameter @float:
                return new FloatField()
                {
                    LabelText = parameter.DisplayName,
                    Value = @float.DefaultValue
                };
            case FloatSliderParameter fslider:
                return new FloatSliderField()
                {
                    LabelText = parameter.DisplayName,
                    Value = fslider.DefaultValue,
                    MinValue = fslider.MinValue,
                    MaxValue = fslider.MaxValue
                };
            case CharParameter @char:
                return new CharField()
                {
                    LabelText = parameter.DisplayName,
                    Value = @char.DefaultValue
                };
            case TextlineParameter line:
                return new LineField()
                {
                    LabelText = parameter.DisplayName,
                    Value = line.DefaultValue
                };
            case MultilineParameter multiline:
                return new MultilineField()
                {
                    LabelText = parameter.DisplayName,
                    Value = multiline.DefaultValue
                };
            case ColorParameter color:
                return new ColorField()
                {
                    LabelText = parameter.DisplayName,
                    Value = color.DefaultValue
                };
            default:
                return null;
        }
    }
}