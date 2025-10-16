namespace Rusty.ISA.Editor;

/// <summary>
/// An utility for getting parameter data.
/// </summary>
public static class ParameterUtility
{
    /// <summary>
    /// Get the default value of a parameter. Returns null if the parameter has no default value.
    /// </summary>
    public static object GetDefaultValue(Parameter parameter)
    {
        switch (parameter)
        {
            case BoolParameter b:
                return b.DefaultValue;
            case IntParameter i:
                return i.DefaultValue;
            case FloatParameter f:
                return f.DefaultValue;
            case IntSliderParameter isl:
                return isl.DefaultValue;
            case FloatSliderParameter fsl:
                return fsl.DefaultValue;
            case CharParameter c:
                return c.DefaultValue;
            case TextlineParameter tl:
                return tl.DefaultValue;
            case MultilineParameter ml:
                return ml.DefaultValue;
            case ColorParameter col:
                return col.DefaultValue;
            default:
                return null;
        }
    }

    /// <summary>
    /// Get the minimum value of a parameter. Returns null if the parameter has no minimum value.
    /// </summary>
    public static object GetMinValue(Parameter parameter)
    {
        switch (parameter)
        {
            case IntSliderParameter isl:
                return isl.MinValue;
            case FloatSliderParameter fsl:
                return fsl.MinValue;
            default:
                return null;
        }
    }

    /// <summary>
    /// Get the maximum value of a parameter. Returns null if the parameter has no maximum value.
    /// </summary>
    public static object GetMaxValue(Parameter parameter)
    {
        switch (parameter)
        {
            case IntSliderParameter isl:
                return isl.MaxValue;
            case FloatSliderParameter fsl:
                return fsl.MaxValue;
            default:
                return null;
        }
    }
}