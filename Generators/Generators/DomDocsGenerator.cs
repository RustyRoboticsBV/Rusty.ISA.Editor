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
                    parents.Append($"<code>{parent.Name}</code>");
                }
            }
            if (parents.IsEmpty)
                parents.Append("<i>(None)</i>");

            CodeWriter children = new();
            foreach (string child in element.Children)
            {
                if (!children.IsEmpty)
                    children.Append(", ");
                children.Append($"<code>{child}</code>");
            }
            if (children.IsEmpty)
                children.Append("<i>(None)</i>");

            CodeWriter attributes = new();
            attributes.Indent = 1;
            attributes.Line($"<table>");
            attributes.Line($"  <tr>");
            attributes.Line($"    <th>Index</th>");
            attributes.Line($"    <th>Name</th>");
            attributes.Line($"    <th>Type</th>");
            attributes.Line($"    <th>Description</th>");
            attributes.Line($"  </tr>");
            for (int j = 0; j < element.Attributes.Count; j++)
            {
                string type = element.Attributes[j].ValueType == ValueType.Ref
                    ? ValueType.String.ToString()
                    : element.Attributes[j].ValueType.ToString();

                attributes.Line($"  <tr>");
                attributes.Line($"    <td><code>{j}</code></td>");
                attributes.Line($"    <td><code>{element.Attributes[j].Name}</code></td>");
                attributes.Line($"    <td><code>{type.ToLowerInvariant()}</code></td>");
                attributes.Line($"    <td>{element.Attributes[j].Description}</td>");
                attributes.Line($"  </tr>");
            }
            attributes.Line($"</table>");

            elementTypes.Line();
            elementTypes.Line($"### `{element.Name}`");
            elementTypes.Line($"<table>");
            elementTypes.Line($"  <tr>");
            elementTypes.Line($"    <th>Property</th>");
            elementTypes.Line($"    <th>Value</th>");
            elementTypes.Line($"  </tr>");
            elementTypes.Line($"  <tr>");
            elementTypes.Line($"    <td>Index</td>");
            elementTypes.Line($"    <td><code>{i}</code></td>");
            elementTypes.Line($"  </tr>");
            elementTypes.Line($"  <tr>");
            elementTypes.Line($"    <td>Description</td>");
            elementTypes.Line($"    <td>{element.Description}</td>");
            elementTypes.Line($"  </tr>");
            elementTypes.Line($"  <tr>");
            elementTypes.Line($"    <td>Parent</td>");
            elementTypes.Line($"    <td>{parents}</td>");
            elementTypes.Line($"  </tr>");
            elementTypes.Line($"  <tr>");
            elementTypes.Line($"    <td>Children</td>");
            elementTypes.Line($"    <td>{children}</td>");
            elementTypes.Line($"  </tr>");
            elementTypes.Line($"    <td>Multiple children allowed?</td>");
            elementTypes.Line($"    <td>{element.MultipleChildren.ToString().ToLowerInvariant()}</td>");
            elementTypes.Line($"  </tr>");
            elementTypes.Line($"  <tr>");
            elementTypes.Line($"    <td>Attributes</td>");
            elementTypes.Line($"    <td>");
            elementTypes.Line(attributes.ToString());
            elementTypes.Line($"    </td>");
            elementTypes.Line($"  </tr>");
            elementTypes.Line($"</table>");
        }

        return @$"# Schema

## General Rules
- Each file must have a `{fileSnake}` element as the root.
- All `{schema.GlobalAttributes[0].Name}` attributes must be unique among elements with the same tag within their scope.

## Element Types
{elementTypes}
";
    }
}