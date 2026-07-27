namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A OadefCodec and OutCodec pair representing an argument output port.
/// </summary>
internal class OutputPortInfo
{
    /* Public properties. */
    public PdefCodec Parameter { get; private set; }
    public OadefCodec Definition { get; private set; }
    public OutCodec Instance { get; private set; }

    /* Constructors. */
    public OutputPortInfo(PdefCodec parameter, OadefCodec definition, OutCodec instance)
    {
        Parameter = parameter;
        Definition = definition;
        Instance = instance;
    }

    /* Public methods. */
    public override string ToString()
    {
        return Parameter.GetAttribute(Codec.ID)
            + " / " + Definition.GetAttribute(Codec.ID)
            + ": \"" + Instance.GetAttribute(Codec.Value) + '"';
    }
}