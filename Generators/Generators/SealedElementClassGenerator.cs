using System.Collections.Generic;

public sealed class SealedElementClassGenerator : Generator
{
    public string Generate(Schema schema, Sealed @sealed)
    {
        bool needGodot = false;

        Class @class = Class(AccessType.Internal, ModifierType.Sealed, Names.ElementName(@sealed.Name));
        @class.Summary = Summary(@sealed.Description);
        if (@sealed.Inherits != null)
            @class.Base = Type(Names.ElementName(@sealed.Inherits.Name));
        else
            @class.Base = Type("Element");

        // Constants.
        @class.Members.Add(MultilineComment("Public constants."));
        @class.Members.Add(new Code($"public const int ELEMENT_INDEX = {schema.Sealeds.IndexOf(@sealed)};"));
        @class.Members.Add(new Code($"public const string ELEMENT_NAME = \"{@sealed.Name}\";"));

        // Properties.
        @class.Members.Add(new Empty());
        @class.Members.Add(MultilineComment("Public properties."));

        @class.Members.Add(Property(
            AccessType.Public,
            ModifierType.Override,
            Type("int"),
            Id("ElementIndex"),
            Getter(Return(Id("ELEMENT_INDEX")))
        ));

        @class.Members.Add(Property(
            AccessType.Public,
            ModifierType.Override,
            Type("string"),
            Id("ElementName"),
            Getter(Return(Id("ELEMENT_NAME")))
        ));

        @class.Members.Add(new Empty());
        @class.Members.Add(Property(
            AccessType.Public,
            ModifierType.Override,
            Type("int"),
            Id("AttributeCount"),
            Getter(Return(Int(@sealed.Attributes.Count)))
        ));

        foreach (Attribute attribute in @sealed.Attributes)
        {
            bool isShared = schema.HasGlobalAttribute(attribute)
                || (@sealed.Inherits != null ? @sealed.Inherits.HasAttribute(attribute) : false);

            @class.Members.Add(Property(
                Summary(attribute.Description),
                AccessType.Public,
                isShared ? ModifierType.Override : ModifierType.None,
                Types.Get(attribute.ValueType),
                Id(Names.CamelCase(attribute.Name)),
                Getter(),
                Setter(),
                attribute.ValueType >= ValueType.String ? String("") : null
            ));

            if (attribute.ValueType == ValueType.Color)
                needGodot = true;
        }

        @class.Members.Add(new Empty());
        @class.Members.Add(Property(
            AccessType.Public,
            ModifierType.Override,
            Type("bool"),
            Id("AllowsChildren"),
            Getter(Return(Bool(@sealed.Children.Count > 0)))
        ));

        // Methods.
        @class.Members.Add(new Empty());
        @class.Members.Add(MultilineComment("Public methods."));

        List<Case> indexOfAttribute = new();
        foreach (Attribute attribute in @sealed.Attributes)
        {
            String name = String(attribute.Name);
            Int index = Int(indexOfAttribute.Count);
            indexOfAttribute.Add(Case(name, Return(index)));
        }

        @class.Members.Add(Method(
            AccessType.Public,
            ModifierType.Override,
            Type("int"),
            Id("IndexOfAttribute"),
            Parameter(Type("string"), Id("name")),
            Switch(
                Id("name"),
                Default(Throw(Type("ArgumentOutOfRangeException"), IString("No attribute corresponds to the name '{name}'."))),
                indexOfAttribute.ToArray()
            )
        ));


        @class.Members.Add(new Empty());
        List<Case> nameOfAttribute = new();
        foreach (Attribute attribute in @sealed.Attributes)
        {
            Int index = Int(nameOfAttribute.Count);
            String str = String(attribute.Name);
            nameOfAttribute.Add(Case(index, Return(str)));
        }
        
        @class.Members.Add(new Empty());
        @class.Members.Add(Method(
            AccessType.Public,
            ModifierType.Override,
            Type("string"),
            Id("NameOfAttribute"),
            Parameter(Type("int"), Id("index")),
            Switch(
                Id("index"),
                Default(Throw(Type("ArgumentOutOfRangeException"), IString("No attribute corresponds to the index {index}."))),
                nameOfAttribute.ToArray()
            )
        ));


        List<Case> typeOfAttribute = new();
        foreach (Attribute attribute in @sealed.Attributes)
        {
            Int index = Int(typeOfAttribute.Count);
            Id type = Id(Names.CamelCase(attribute.Name) + ".GetType()");
            typeOfAttribute.Add(Case(index, Return(type)));
        }

        @class.Members.Add(new Empty());
        @class.Members.Add(Method(
            AccessType.Public,
            ModifierType.Override,
            Type("Type"),
            Id("TypeOfAttribute"),
            Parameter(Type("int"), Id("index")),
            Switch(
                Id("index"),
                Default(Throw(Type("ArgumentOutOfRangeException"), IString("No attribute corresponds to the index {index}."))),
                typeOfAttribute.ToArray()
            )
        ));


        List<Case> getAttribute = new();
        foreach (Attribute attribute in @sealed.Attributes)
        {
            Int index = Int(getAttribute.Count);
            Id id = Id(Names.CamelCase(attribute.Name));
            getAttribute.Add(Case(index, Return(id)));
        }

        @class.Members.Add(new Empty());
        @class.Members.Add(Method(
            AccessType.Public,
            ModifierType.Override,
            Type("object"),
            Id("GetAttribute"),
            Parameter(Type("int"), Id("index")),
            Switch(
                Id("index"),
                Default(Throw(Type("ArgumentOutOfRangeException"), IString("No attribute corresponds to the index {index}."))),
                getAttribute.ToArray()
            )
        ));


        List<Case> setAttribute = new();
        foreach (Attribute attribute in @sealed.Attributes)
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
                Default(Throw(Type("ArgumentOutOfRangeException"), IString("No attribute corresponds to the index {index}."))),
                setAttribute.ToArray()
            )
        ));


        List<Case> addChild = new();
        foreach (string child in @sealed.Children)
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
            @sealed.Children.Count > 0
                ? Switch(
                    Id("child"),
                    Default(Throw(Type("ArgumentOutOfRangeException"), IString("Elements of type {GetType().Name} do not allow children of type {child.GetType().Name}."))),
                    addChild.ToArray()
                )
                : Throw(Type("NotImplementedException"), IString("Elements of type {GetType().Name} do not allow child elements."))
        ));

        if (@sealed.Children.Count > 0 && !@sealed.MultipleChildren)
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