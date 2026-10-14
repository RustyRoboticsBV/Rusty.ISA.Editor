using System;

namespace Rusty.ActionGraph.Editor;

internal static class FieldCreator
{
    public static IField Create(ParameterDefinition definition)
    {
        switch (definition)
        {
            case BoolDefinition @bool:
                return new BoolField(@bool.Title, @bool.DefaultValue);
            case NumericDefinition num:
                return new NumericField(num.Title, num.DefaultValue, num.MinValue, num.MaxValue, num.Step);
            case StringDefinition str:
                if (str.Lines == 1)
                    return new LineField(str.Title, str.DefaultValue);
                else
                    return new TextField(str.Title, str.DefaultValue, str.Lines);
            case ColorDefinition color:
                return new ColorField(color.Title, color.DefaultValue);
            case OutputDefinition:
                return null;
            default:
                throw new Exception("Unknown parameter type.");
        }
    }
}