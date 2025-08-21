namespace Rusty.ISA.Editor;

/// <summary>
/// A parameter inspector.
/// </summary>
public partial class ParameterInspector : Inspector
{
    /* Public properties. */
    public Parameter Parameter { get; private set; }

    public virtual object Value
    {
        get => Field?.Value;
        set
        {
            if (Field != null)
                Field.Value = value;
        }
    }

    public ParameterPreviewInstance Preview { get; private set; }

    /* Private properties. */
    private IField Field => GetAt(0) as IField;

    /* Constructors. */
    public ParameterInspector(Parameter parameter)
    {
        Parameter = parameter;

        // Create field.
        IField field = CreateField(parameter);
        if (field != null)
        {
            Add("field", field);
            field.Name = "Field";
        }

        // Preview.
        Preview = PreviewDict.ForParameter(parameter)?.CreateInstance();
        Changed += UpdatePreview;
    }

    /* Public methods. */
    public override void CopyFrom(IGuiElement other)
    {
        base.CopyFrom(other);

        if (other is ParameterInspector inspector)
            Parameter = inspector.Parameter;
    }

    public void ParseValue(string value)
    {
        switch (Field)
        {
            case BoolField boolField:
                boolField.Value = StringUtility.ParseBool(value);
                break;
            case IntField intField:
                intField.Value = StringUtility.ParseInt(value);
                break;
            case FloatField floatField:
                floatField.Value = StringUtility.ParseFloat(value);
                break;
            case IntSliderField intSlider:
                intSlider.Value = StringUtility.ParseInt(value);
                break;
            case FloatSliderField floatSlider:
                floatSlider.Value = StringUtility.ParseFloat(value);
                break;
            case CharField charField:
                charField.Value = StringUtility.ParseChar(value);
                break;
            case LineField lineField:
                lineField.Value = value;
                break;
            case MultilineField multilineField:
                multilineField.Value = value;
                break;
            case ColorField colorField:
                colorField.Value = StringUtility.ParseColor(value);
                break;
        }
    }

    /* Protected methods. */
    protected virtual void UpdatePreview()
    {
        if (Field != null)
            Preview?.SetValue(Field.Value);
        else
            Preview?.SetValue("");
        Godot.GD.Print(Parameter.ID + " " + Preview);
    }

    /* Private methods. */
    private static IField CreateField(Parameter parameter)
    {
        IField field;
        switch (parameter)
        {
            case BoolParameter b:
                field = new BoolField()
                {
                    Value = b.DefaultValue
                };
                break;
            case IntParameter i:
                field = new IntField()
                {
                    Value = i.DefaultValue
                };
                break;
            case IntSliderParameter isl:
                field = new IntSliderField()
                {
                    Value = isl.DefaultValue,
                    MinValue = isl.MinValue,
                    MaxValue = isl.MaxValue
                };
                break;
            case FloatParameter f:
                field = new FloatField()
                {
                    Value = f.DefaultValue
                };
                break;
            case FloatSliderParameter fsl:
                field = new FloatSliderField()
                {
                    Value = fsl.DefaultValue,
                    MinValue = fsl.MinValue,
                    MaxValue = fsl.MaxValue
                };
                break;
            case CharParameter c:
                field = new CharField()
                {
                    Value = c.DefaultValue
                };
                break;
            case TextlineParameter l:
                field = new LineField()
                {
                    Value = l.DefaultValue
                };
                break;
            case MultilineParameter ml:
                field = new MultilineField()
                {
                    Value = ml.DefaultValue
                };
                break;
            case ColorParameter col:
                field = new ColorField()
                {
                    Value = col.DefaultValue
                };
                break;
            default:
                field = null;
                break;
        }

        if (field != null)
        {
            field.LabelText = parameter.DisplayName;
            field.TooltipText = parameter.Description;
        }

        return field;
    }
}