using System.Text;

public class Attribute
{
    /* Public properties. */
    public string Name { get; set; }
    public string Description { get; set; }
    public ValueType ValueType { get; set; }
    public Element RefType { get; set; }

    /* Public methods. */
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(ValueType);
        if (ValueType == ValueType.Ref && RefType != null)
        {
            sb.Append(" (");
            sb.Append(RefType.Name);
            sb.Append(")");
        }
        sb.Append(' ');
        sb.Append(Name);
        if (Description != null)
        {
            sb.Append("    // ");
            sb.Append(Description);
        }
        return sb.ToString();
    }
}