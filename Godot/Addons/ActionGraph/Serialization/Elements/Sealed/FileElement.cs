////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated file. It should not be edited directly.                //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A definition for an option inspector.
/// </summary>
internal sealed class FileElement : Element
{
    /* Public constants. */
    public const int ELEMENT_INDEX = 0;
    public const string ELEMENT_NAME = "file";
    
    /* Public properties. */
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    
    public override int AttributeCount => 5;
    /// <summary>
    /// The unique identifier of this file.
    /// </summary>
    public override string ID { get; set; } = "";
    /// <summary>
    /// The name of the editor that generated this file.
    /// </summary>
    public string Editor { get; set; } = "";
    /// <summary>
    /// The ActionGraph schema version.
    /// </summary>
    public string SchemaVersion { get; set; } = "";
    /// <summary>
    /// The ID of the default language when no localization is applied. This must NOT match any language definition IDs.
    /// </summary>
    public string DefaultLanguage { get; set; } = "";
    /// <summary>
    /// The checksum of this file.
    /// </summary>
    public string Checksum { get; set; } = "";
    
    public override bool AllowsChildren => true;
    
    /* Public methods. */
    public override int IndexOfAttribute(string name) => name switch
    {
        "id" => 0,
        "editor" => 1,
        "schema_version" => 2,
        "default_language" => 3,
        "checksum" => 4,
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.")
    };
    
    
    public override string NameOfAttribute(int index) => index switch
    {
        0 => "id",
        1 => "editor",
        2 => "schema_version",
        3 => "default_language",
        4 => "checksum",
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override Type TypeOfAttribute(int index) => index switch
    {
        0 => ID.GetType(),
        1 => Editor.GetType(),
        2 => SchemaVersion.GetType(),
        3 => DefaultLanguage.GetType(),
        4 => Checksum.GetType(),
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override object GetAttribute(int index) => index switch
    {
        0 => ID,
        1 => Editor,
        2 => SchemaVersion,
        3 => DefaultLanguage,
        4 => Checksum,
        _ => throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.")
    };
    
    public override void SetAttribute(int index, object value)
    {
        switch (index)
        {
            case 0:
                ID = (string)value;
                break;
            case 1:
                Editor = (string)value;
                break;
            case 2:
                SchemaVersion = (string)value;
                break;
            case 3:
                DefaultLanguage = (string)value;
                break;
            case 4:
                Checksum = (string)value;
                break;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override void AddChild(Element child)
    {
        switch (child)
        {
            case MetadataElement:
            case LanguageDefinitionElement:
            case InstructionDefinitionElement:
            case NodeDefinitionElement:
            case SubgraphDefinitionElement:
            case StartPointElement:
            case NodeElement:
            case FrameElement:
            case MemoElement:
            case SubgraphElement:
            case EdgeElement:
                Children.Add(child);
                break;
            default:
                throw new ArgumentOutOfRangeException($"Elements of type '{ElementName}' cannot have children of type '{child.ElementName}'.");
        }
    }
}