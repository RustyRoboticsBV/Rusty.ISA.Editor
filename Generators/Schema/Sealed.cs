using System.Collections.Generic;
using System.Text;

public sealed class Sealed : Element
{
    /* Public properties. */
    public string Color { get; set; }
    public Abstract Inherits { get; set; }
    public List<string> Children { get; set; } = new();
    public bool MultipleChildren { get; set; }

    /* Public methods. */
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(Name);
        if (Inherits != null)
        {
            sb.Append(" : ");
            sb.Append(Inherits.Name);
        }
        if (Description != "")
        {
            sb.Append("  // ");
            sb.Append(Description);
        }

        sb.Append("\nAttributes:");
        foreach (var attribute in Attributes)
        {
            sb.Append("\n- ");
            sb.Append(attribute.ToString());
        }

        if (Children.Count > 0)
        {
            sb.Append("\n");
            sb.Append("Children:");
            foreach (var child in Children)
            {
                sb.Append("\n- ");
                sb.Append(child);
            }
            if (MultipleChildren)
            {
                sb.Append("\n");
                sb.Append("Multiple children: ");
                sb.Append(MultipleChildren);
            }
        }

        return sb.ToString();
    }

    public override Element GetBase() => Inherits;
}