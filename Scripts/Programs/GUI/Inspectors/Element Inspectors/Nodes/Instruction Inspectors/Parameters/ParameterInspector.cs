namespace Rusty.ISA.Editor;

/// <summary>
/// A parameter inspector.
/// </summary>
public partial class ParameterInspector : ResourceInspector
{
    /* Public properties. */
    public InstructionDefinition Definition { get; private set; }
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

    public new ParameterPreviewInstance Preview
    {
        get => base.Preview as ParameterPreviewInstance;
        set => base.Preview = value;
    }

    /* Private properties. */
    private IField Field => GetContentsCount() > 0 ?  GetAt(0) as IField : null;

    /* Constructors. */
    public ParameterInspector(InstructionSet set, string opcode, string parameterID) : base(set)
    {
        // Store data.
        Definition = set[opcode];
        Parameter = Definition.GetParameter(parameterID);

        // Create field.
        IField field = CreateField(Parameter);
        if (field != null)
        {
            Add("field", field);
            field.Name = "Field";
        }

        // Enable preview.
        EnablePreview();
    }

    /* Public methods. */
    public override IGuiElement Copy()
    {
        ParameterInspector copy = new(InstructionSet, Definition.Opcode, Parameter.ID);
        copy.CopyFrom(this);
        return copy;
    }

    public override void CopyFrom(IGuiElement other)
    {
        DisablePreview();

        // Base resource inspector copy.
        base.CopyFrom(other);

        // Copy parameter.
        if (other is ParameterInspector inspector)
            Parameter = inspector.Parameter;

        EnablePreview();
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
    protected override void UpdatePreview()
    {
        // Init.
        if (Preview == null)
            Preview = PreviewDict.ForParameter(InstructionSet[Definition.Opcode], Parameter.ID).CreateInstance();

        // Update.
        if (Preview != null && Field != null)
            Preview.SetValue(Field.Value);
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