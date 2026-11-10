public class DomDocsGenerator : Generator
{
    public string Generate(Schema schema)
    {
        string fileSnake = schema.Sealeds[0].Name;
        string fileElement = Names.ElementName(fileSnake);

        CodeWriter elementTypes = new();
        for (int i = 0; i < schema.Sealeds.Count; i++)
        {
            Sealed element = schema.Sealeds[i];

            CodeWriter parents = new();
            foreach (Sealed parent in schema.Sealeds)
            {
                if (parent.Children.Contains(element.Name))
                {
                    if (!parents.IsEmpty)
                        parents.Append(", ");
                    parents.Append($"`{parent.Name}`");
                }
            }
            if (parents.IsEmpty)
                parents.Append("*(None)*");

            CodeWriter children = new();
            foreach (string child in element.Children)
            {
                if (!children.IsEmpty)
                    children.Append(", ");
                children.Append($"`{child}`");
            }
            if (children.IsEmpty)
                children.Append("*(None)*");

            elementTypes.Line();
            elementTypes.Line($"### `{element.Name}`");
            elementTypes.Line($"|Property|Value|");
            elementTypes.Line($"|-|-|");
            elementTypes.Line($"|Index|`{i}` (`0x{i:X2}`)|");
            elementTypes.Line($"|Description|{element.Description}|");
            elementTypes.Line($"|Allowed parents|{parents}|");
            elementTypes.Line($"|Allowed children|{children}|");
            elementTypes.Line($"|Multiple children allowed?|{(element.MultipleChildren ? "Yes" : "No")}|");

            elementTypes.Line();
            elementTypes.Line("**Attributes:**");
            elementTypes.Line($"|Index|Name|Type|Description|");
            elementTypes.Line($"|-|-|-|-|");
            for (int j = 0; j < element.Attributes.Count; j++)
            {
                string type = element.Attributes[j].ValueType == ValueType.Ref
                    ? ValueType.String.ToString()
                    : element.Attributes[j].ValueType.ToString();

                elementTypes.Line($"|`{j}` (`0x{j:X2}`)|`{element.Attributes[j].Name}`|`{type.ToLowerInvariant()}`|"
                    + $"{element.Attributes[j].Description}|");
            }
            elementTypes.Line("---");
        }

        return @$"# ActionGraph Program Schema

This schema defines the structure of ActionGraph files, including its supported element types, attributes, and hierarchy.
It applies to both [XML](<XML Programs.md>) and [binary](<Binary Programs.md>) files.

## General Rules
- Each file must have a `{fileSnake}` element as the root.
- All `{schema.GlobalAttributes[0].Name}` attributes must be unique among sibling elements with the same parent.

## Element Types{elementTypes}";
    }
}