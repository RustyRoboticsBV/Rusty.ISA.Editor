using System.Collections.Generic;

public sealed class SealedElementClassGenerator : Generator
{
    public string Generate(Schema schema, Sealed element)
    {
        bool needGodot = false;

        Class @class = Class(AccessType.Internal, ModifierType.Sealed, Names.CamelCase(element.Name) + "Element");
        @class.Summary = Summary(element.Description);
        if (element.Inherits != null)
            @class.Base = Type(Names.ElementName(element.Inherits.Name));
        else
            @class.Base = Type("Element");

        // Properties.
        @class.Members.Add(MultilineComment("Public properties."));

        @class.Members.Add(Property(
            AccessType.Public,
            ModifierType.Override,
            Type("int"),
            Id("AttributeCount"),
            Getter(Return(Int(element.Attributes.Count)))
        ));

        @class.Members.Add(new Empty());
        foreach (Attribute attribute in element.Attributes)
        {
            bool isShared = schema.HasGlobalAttribute(attribute)
                || (element.Inherits != null ? element.Inherits.HasAttribute(attribute) : false);

            @class.Members.Add(Property(
                Summary(attribute.Description),
                AccessType.Public,
                isShared ? ModifierType.Override : ModifierType.None,
                Types.Get(attribute.ValueType),
                Id(Names.CamelCase(attribute.Name)),
                Getter(),
                Setter()
            ));

            if (attribute.ValueType == ValueType.Color)
                needGodot = true;
        }

        // Methods.
        @class.Members.Add(new Empty());
        @class.Members.Add(MultilineComment("Public methods."));

        List<Case> getAttribute = new();
        foreach (Attribute attribute in element.Attributes)
        {
            Int index = Int(getAttribute.Count);
            Id id = Id(Names.CamelCase(attribute.Name));
            getAttribute.Add(Case(index, Return(id)));
        }

        @class.Members.Add(Method(
            AccessType.Public,
            ModifierType.Override,
            Type("object"),
            Id("GetAttribute"),
            Parameter(Type("int"), Id("index")),
            Switch(
                Id("index"),
                Default(Throw(Type("ArgumentOutOfRangeException"), IString("No attribute corresponds to index {index}."))),
                getAttribute.ToArray()
            )
        ));


        List<Case> setAttribute = new();
        foreach (Attribute attribute in element.Attributes)
        {
            Int index = Int(setAttribute.Count);
            Id id = Id(Names.CamelCase(attribute.Name));
            TypeName type = Types.Get(attribute.ValueType);
            setAttribute.Add(Case(index, Assign(id, Cast(type, Id("value")))));
        }

        @class.Members.Add(new Empty());
        @class.Members.Add(Method(
            AccessType.Public,
            ModifierType.Override,
            Type("void"),
            Id("SetAttribute"),
            Parameter(Type("int"), Id("index")),
            Parameter(Type("object"), Id("value")),
            Switch(
                Id("index"),
                Default(Throw(Type("ArgumentOutOfRangeException"), IString("No attribute corresponds to index {index}."))),
                setAttribute.ToArray()
            )
        ));


        List<Case> addChild = new();
        foreach (string child in element.Children)
        {
            Id type = Id(Names.ElementName(child));
            addChild.Add(Case(type, []));
            addChild[^1].OmitBreak = true;
        }

        if (addChild.Count > 0)
        {
            addChild[^1].Statements.Add(new CallStatement()
            {
                Call = new()
                {
                    Name = "Children.Add",
                    Arguments = new()
                    {
                        Arguments = [Id("child")]
                    }
                }
            });
            addChild[^1].OmitBreak = false;
        }

        @class.Members.Add(new Empty());
        @class.Members.Add(Method(
            AccessType.Public,
            ModifierType.Override,
            Type("void"),
            Id("AddChild"),
            Parameter(Type("Element"), Id("child")),
            element.Children.Count > 0
                ? Switch(
                    Id("child"),
                    Default(Throw(Type("ArgumentOutOfRangeException"), IString("Elements of type {GetType().Name} do not allow children of type {child.GetType().Name}."))),
                    addChild.ToArray()
                )
                : Throw(Type("NotImplementedException"), IString("Elements of type {GetType().Name} do not allow child elements."))
        ));

        if (element.Children.Count > 0 && !element.MultipleChildren)
        {
            var s = If(Greater(Id("Children.Count"), Int(0)),
                Throw(Type("InvalidOperationException"), IString("Elements of type {GetType().Name} do not allow multiple child elements."))
            );
            ((Method)@class.Members[^1]).Implementation.Statements.Insert(0, s);
            ((Method)@class.Members[^1]).Implementation.Statements.Insert(1, new Empty());
        }

        CodeWriter w = new();
        w.Line(Notice);
        w.Line();
        if (needGodot)
            Using("Godot").Generate(w);
        Using("System").Generate(w);
        w.Line();
        Namespace("Rusty", "ActionGraph", "Serialization").Generate(w);
        w.Line();
        w.Line();
        @class.Generate(w);
        return w.ToString();
    }
}