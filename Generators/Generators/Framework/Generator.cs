using System;

public abstract class Generator
{
    /* Protected properties. */
    protected static string Notice =>
@"////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////";

    /* Protected methods. */
    protected static Id Id(string name) => new(name);

    protected static TypeName Type(string name) => new(name);

    protected static Using Using(params Id[] names) => new() { Namespace = new() { Names = names } };

    protected static Namespace Namespace(params Id[] names) => new() { Names = names };

    protected static Summary Summary(string text) => new() { Text = text };

    protected static MultilineComment MultilineComment(string text) => new() { Text = text };

    protected static Class Class(Access access, Modifier modifier, Id name, TypeName @base = null) => new()
    {
        Access = access,
        Modifier = modifier,
        Name = name,
        Base = @base
    };

    protected static Property Property(params object[] contents)
    {
        Property prop = new();
        foreach (object node in contents)
        {
            switch (node)
            {
                case Summary summary: prop.Summary = summary; break;
                case Access access: prop.Access = access; break;
                case AccessType access: prop.Access = access; break;
                case Modifier modifier: prop.Modifier = modifier; break;
                case ModifierType modifier: prop.Modifier = modifier; break;
                case TypeName type: prop.Type = type; break;
                case Id name: prop.Name = name; break;
                case Getter getter: prop.Getter = getter; break;
                case Setter setter: prop.Setter = setter; break;
                case Value value: prop.Initialization = value; break;
                default: throw new ArgumentException(node.GetType().Name, nameof(contents));
            }
        }
        return prop;
    }

    protected static Getter Getter(params Statement[] implementation) => new()
    {
        Implementation = new()
        {
            Statements = new(implementation)
        }
    };

    protected static Setter Setter(params Statement[] implementation) => new()
    {
        Implementation = new()
        {
            Statements = new(implementation)
        }
    };

    protected static Method Method(params object[] contents)
    {
        Method method = new();
        foreach (object node in contents)
        {
            switch (node)
            {
                case Summary summary: method.Summary = summary; break;
                case Access access: method.Access = access; break;
                case AccessType access: method.Access = access; break;
                case Modifier modifier: method.Modifier = modifier; break;
                case ModifierType modifier: method.Modifier = modifier; break;
                case TypeName type: method.ReturnType = type; break;
                case Id name: method.Name = name; break;
                case Parameter parameter: method.Parameters.Add(parameter); break;
                case Statement statement: method.Implementation.Statements.Add(statement); break;
                default: throw new ArgumentException(node.GetType().Name, nameof(contents));
            }
        }
        return method;
    }

    protected static Parameter Parameter(TypeName type, Id name) => new(type, name);

    protected static Return Return(Value value) => new() { Value = value };

    protected static Bool Bool(bool value) => new() { Value = value };

    protected static Int Int(int value) => new() { Value = value };

    protected static String String(string value) => new() { Value = value };

    protected static IString IString(string value) => new() { Value = value };

    protected static Switch Switch(Value value, Default @default, params Case[] cases) => new()
    {
        Value = value,
        Default = @default,
        Cases = new(cases)
    };

    protected static Default Default(params Statement[] statements) => new()
    {
        Statements = new(statements)
    };

    protected static Case Case(Value value, params Statement[] statements) => new()
    {
        Value = value,
        Statements = new(statements)
    };

    protected static Throw Throw(TypeName exception, params Value[] arguments) => new()
    {
        Exception = new()
        {
            Name = exception.Name,
            Arguments = new(arguments)
        }
    };

    protected static If If(Value condition, params Statement[] statements) => new()
    {
        Condition = condition,
        Statements = new(statements)
    };

    protected static Else Else(params Statement[] statements) => new()
    {
        Statements = new(statements)
    };

    protected static Cast Cast(TypeName type, Value value) => new()
    {
        Type = type,
        Value = value
    };

    protected static Assign Assign(Id name, Value value) => new()
    {
        Name = name,
        Value = value
    };

    protected static Less Less(Value left, Value right) => new()
    {
        Left = left,
        Right = right
    };

    protected static GreaterEqual GreaterEqual(Value left, Value right) => new()
    {
        Left = left,
        Right = right
    };

    protected static Or Or(Value left, Value right) => new()
    {
        Left = left,
        Right = right
    };

    protected static For For(TypeName type, Id name, Value from, Value to, params Statement[] statements) => new()
    {
        Type = type,
        Name = name,
        From = from,
        To = to,
        Statements = new(statements)
    };
}