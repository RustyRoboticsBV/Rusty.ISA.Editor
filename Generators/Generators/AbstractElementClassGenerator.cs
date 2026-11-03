public sealed class AbstractElementClassGenerator : Generator
{
    public string Generate(Schema schema, Abstract @abstract)
    {
        bool needGodot = false;

        Class @class = Class(AccessType.Internal, ModifierType.Abstract, Names.CamelCase(@abstract.Name) + "Element");
        @class.Summary = Summary(@abstract.Description);
        @class.Base = Type("Element");

        // Properties.
        bool first = true;
        foreach (Attribute shared in @abstract.Attributes)
        {
            if (shared.ValueType == ValueType.Color)
                needGodot = true;

            bool isGlobal = false;
            foreach (Attribute globalShared in schema.GlobalAttributes)
            {
                if (globalShared.ValueType == shared.ValueType && globalShared.Name == shared.Name)
                    isGlobal = true;
            }
            if (isGlobal)
                continue;

            if (first)
                @class.Members.Add(MultilineComment("Public properties."));
            first = false;

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

        CodeWriter w = new();
        w.Line(Notice);
        if (needGodot)
        {
            w.Line();
            Using("Godot").Generate(w);
        }
        w.Line();
        Namespace("Rusty", "ActionGraph", "Serialization").Generate(w);
        w.Line();
        w.Line();
        @class.Generate(w);
        return w.ToString();
    }
}