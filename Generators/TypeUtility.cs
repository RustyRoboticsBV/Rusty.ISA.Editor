using System;

internal class TypeUtility
{
    public static string GetType(ValueType valueType)
    {
        switch (valueType)
        {
            case ValueType.Bool:
                return "bool";
            case ValueType.Int:
                return "int";
            case ValueType.Uint:
                return "uint";
            case ValueType.String:
                return "string";
            case ValueType.Color:
                return "Godot.Color";
            case ValueType.Ref:
                return "string";
            default:
                throw new Exception();
        }
    }
}