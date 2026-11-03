using System.Collections.Generic;

public abstract class Element
{
    /* Public properties. */
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Attribute> Attributes { get; set; } = new();

    /* Public methods. */
    public virtual Element GetBase() => null;
}
