public sealed class BaseElementClassGenerator : Generator
{
    public string Generate(Schema schema)
    {
        Class @class = Class(AccessType.Internal, ModifierType.Abstract, "Element");
        @class.Summary = Summary("A serializer element, representing a node in the ActionGraph document tree.");

        // Properties.
        @class.Members.Add(MultilineComment("Public properties."));

        @class.Members.Add(Property(
            Summary("The index of this element type."),
            AccessType.Public,
            ModifierType.Abstract,
            Type("int"),
            Id("ElementIndex"),
            Getter()
        ));
        @class.Members.Add(Property(
            Summary("The name of this element type."),
            AccessType.Public,
            ModifierType.Abstract,
            Type("string"),
            Id("ElementName"),
            Getter()
        ));

        @class.Members.Add(Property(
            Summary("The number of attributes of this element."),
            AccessType.Public,
            ModifierType.Abstract,
            Type("int"),
            Id("AttributeCount"),
            Getter()
        ));

        foreach (Attribute shared in schema.GlobalAttributes)
        {
            @class.Members.Add(Property(
                Summary(shared.Description),
                AccessType.Public,
                ModifierType.Abstract,
                Types.Get(shared.ValueType),
                Id(Names.CamelCase(shared.Name)),
                Getter(),
                Setter()
            ));
        }

        @class.Members.Add(new Empty());
        @class.Members.Add(Property(
            Summary("The number of child elements contained within this element."),
            AccessType.Public,
            Type("int"),
            Id("ChildCount"),
            Getter(Return(Id("Children.Count")))
        ));


        @class.Members.Add(new Empty());
        @class.Members.Add(MultilineComment("Protected properties."));

        @class.Members.Add(Property(
            Summary("The child elements."),
            AccessType.Protected,
            ModifierType.None,
            Type("List<Element>"),
            Id("Children"),
            Getter(),
            new New()
        ));

        // Methods.
        @class.Members.Add(new Empty());
        @class.Members.Add(MultilineComment("Public methods."));

        @class.Members.Add(Method(
            Summary("Get the name of an attribute by its index."),
            AccessType.Public,
            ModifierType.Abstract,
            Type("string"),
            Id("NameOfAttribute"),
            Parameter(Type("int"), Id("index"))
        ));

        @class.Members.Add(new Empty());
        @class.Members.Add(Method(
            Summary("Get the type of an attribute by its index."),
            AccessType.Public,
            ModifierType.Abstract,
            Type("Type"),
            Id("TypeOfAttribute"),
            Parameter(Type("int"), Id("index"))
        ));

        @class.Members.Add(new Empty());
        @class.Members.Add(Method(
            Summary("Get the index of an attribute by its name."),
            AccessType.Public,
            ModifierType.Abstract,
            Type("int"),
            Id("IndexOfAttribute"),
            Parameter(Type("string"), Id("name"))
        ));

        @class.Members.Add(new Empty());
        @class.Members.Add(Method(
            Summary("Get the value of an attribute by its index."),
            AccessType.Public,
            ModifierType.Abstract,
            Type("object"),
            Id("GetAttribute"),
            Parameter(Type("int"), Id("index"))
        ));

        @class.Members.Add(new Empty());
        @class.Members.Add(Method(
            Summary("Set the value of an attribute by its index."),
            AccessType.Public,
            ModifierType.Abstract,
            Type("void"),
            Id("SetAttribute"),
            Parameter(Type("int"), Id("index")),
            Parameter(Type("object"), Id("value"))
        ));

        @class.Members.Add(new Empty());
        @class.Members.Add(Method(
            Summary("Add a child node."),
            AccessType.Public,
            ModifierType.Abstract,
            Type("void"),
            Id("AddChild"),
            Parameter(Type("Element"), Id("child"))
        ));

        @class.Members.Add(new Empty());
        @class.Members.Add(Method(
            Summary("Get a child node by its index."),
            AccessType.Public,
            Type("Element"),
            Id("GetChild"),
            Parameter(Type("int"), Id("index")),
            If(
                Or(Less(Id("index"), Int(0)), GreaterEqual(Id("index"), Id("ChildCount"))),
                Throw(Type("ArgumentOutOfRangeException"), IString("The child index {index} was out of range (0-{ChildCount})."))
            ),
            Return(Id("Children[index]"))
        ));

        CodeWriter w = new();
        w.Line(Notice);
        w.Line();
        Using("System").Generate(w);
        Using("System", "Collections", "Generic").Generate(w);
        w.Line();
        Namespace("Rusty", "ActionGraph", "Serialization").Generate(w);
        w.Line();
        w.Line();
        @class.Generate(w);
        return w.ToString();
    }
}