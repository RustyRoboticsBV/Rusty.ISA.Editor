using System.Collections.Generic;

public sealed class SealedElementClassGenerator : Generator
{
    public string Generate(Schema schema, Element element)
    {
        bool needGodot = false;

        Class @class = Class(AccessType.Internal, ModifierType.Sealed, Names.CamelCase(element.Name) + "Element");
        @class.Summary = Summary(element.Description);
        if (element.Inherits != null)
            @class.Base = Type(Names.GetCodecName(element.Inherits.Name));
        else
            @class.Base = Type("Element");

        // Properties.
        @class.Members.Add(MultilineComment("Public properties."));

        @class.Members.Add(Property(
            Summary("The number of attributes of this element."),
            AccessType.Public,
            ModifierType.Override,
            Type("int"),
            Id("AttributeCount"),
            Getter(Return(Int(element.Attributes.Count)))
        ));

        @class.Members.Add(new Empty());
        foreach (Attribute attribute in element.Attributes)
        {
            bool isShared = schema.HasSharedAttribute(attribute)
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


        @class.Members.Add(new Empty());
        @class.Members.Add(MultilineComment("Protected properties."));

        @class.Members.Add(Property(
            AccessType.Protected,
            ModifierType.Override,
            Type("bool"),
            Id("AllowsMultipleChildren"),
            Getter(Return(Bool(element.MultipleChildren)))
        ));

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
            Id type = Id(Names.GetCodecName(child));
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
            Switch(
                Id("child"),
                Default(Throw(Type("ArgumentOutOfRangeException"), IString("Elements of type {GetType().Name} do not allow children of type {child.GetType().Name}."))),
                addChild.ToArray()
            )
        ));


        CodeWriter w = new();
        w.Line(Notice);
        w.Line();
        if (needGodot)
        {
            w.Line();
            Using("Godot").Generate(w);
        }
        Using("System").Generate(w);
        w.Line();
        Namespace("Rusty", "ActionGraph", "Serialization").Generate(w);
        w.Line();
        w.Line();
        @class.Generate(w);
        return w.ToString();
    }
}