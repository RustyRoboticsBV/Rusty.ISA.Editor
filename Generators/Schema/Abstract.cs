using System.Text;

public sealed class Abstract : Element
{
    /* Public methods. */
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(Name);

        sb.Append("\nAttributes:");
        foreach (var attribute in Attributes)
        {
            sb.Append("\n- ");
            sb.Append(attribute.ToString());
        }
        return sb.ToString();
    }

    public bool HasAttribute(Attribute attribute)
    {
        foreach (var shared in Attributes)
        {
            if (shared.Name == attribute.Name && shared.ValueType == attribute.ValueType)
                return true;
        }
        return false;
    }
}