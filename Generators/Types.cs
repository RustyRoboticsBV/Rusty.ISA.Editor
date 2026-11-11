using System;

internal class Types
{
    public static string Get(ValueType valueType) => valueType switch
    {
        ValueType.Bool => "bool",
        ValueType.Int => "int",
        ValueType.Uint => "uint",
        ValueType.Double => "double",
        ValueType.String => "string",
        ValueType.Color => "Color",
        ValueType.Ref => "string",
        _ => throw new ArgumentException("Invalid type.", nameof(valueType))
    };

    public static string GetDefault(ValueType valueType) => valueType switch
    {
        ValueType.Bool => null,
        ValueType.Int => null,
        ValueType.Uint => null,
        ValueType.Double => null,
        ValueType.String => "\"\"",
        ValueType.Color => null,
        ValueType.Ref => "\"\"",
        _ => throw new ArgumentException("Invalid type.", nameof(valueType))
    };
}