public class AbstractClassGenerator : ClassGenerator
{
    public static string Generate(Schema schema, Abstract @abstract)
    {
        bool needsGodot = false;

        // Class.
        Class @class = new()
        {
            Access = AccessType.Internal,
            Modifier = ModifierType.Abstract,
            Name = Names.GetCodecName(@abstract.Name),
            Base = "Codec"
        };

        // Properties.
        foreach (var attribute in @abstract.Attributes)
        {
            if (!schema.HasSharedAttribute(attribute))
            {
                if (attribute.ValueType == ValueType.Color)
                    needsGodot = true;

                @class.Members.Add(new Property()
                {
                    Modifier = ModifierType.Abstract,
                    Type = Types.Get(attribute.ValueType),
                    Name = Names.CamelCase(attribute.Name),
                    Getter = new(),
                    Setter = new()
                });
            }
        }

        if (@class.Members.Count > 0)
            @class.Members.Insert(0, new MultilineComment("Public properties."));

        CodeWriter w = new();
        w.Line(Notice);
        w.Line();
        if (needsGodot)
        {
            w.Line("using Godot;");
            w.Line();
        }
        w.Line(Namespace);
        w.Line();
        w.Line();
        @class.Generate(w);

        return w.ToString();
    }
}
