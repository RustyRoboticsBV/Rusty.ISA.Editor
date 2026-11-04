using Rusty.ActionGraph.Serialization;

namespace Rusty.ActionGraph.CodeGen;

/// <summary>
/// A OadefCodec and OutCodec pair representing an argument output port.
/// </summary>
internal sealed class OutputPortInfo
{
    /* Public properties. */
    public ParameterDefinitionElement Parameter { get; private set; }
    public PortDefinitionElement Definition { get; private set; }
    public PortElement Instance { get; private set; }

    /* Constructors. */
    public OutputPortInfo(ParameterDefinitionElement parameter, PortDefinitionElement definition, PortElement instance)
    {
        Parameter = parameter;
        Definition = definition;
        Instance = instance;
    }

    /* Public methods. */
    public override string ToString() => $"{Parameter.ID} / {Definition.ID}: \"{Instance.Index}\"";
}