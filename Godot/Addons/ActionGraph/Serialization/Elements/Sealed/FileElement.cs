////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// The root element.
/// </summary>
internal sealed class FileElement : Element
{
    /* Public properties. */
    public const int ELEMENT_INDEX = 0;
    public const string ELEMENT_NAME = "file";
    
    public override int ElementIndex => ELEMENT_INDEX;
    public override string ElementName => ELEMENT_NAME;
    public override int AttributeCount => 6;
    
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
    /// The file format version.
    /// </summary>
    public string FormatVersion { get; set; } = "";
    /// <summary>
    /// The ID of the default language when no localization is applied. This must NOT match any language_definition IDs.
    /// </summary>
    public string DefaultLanguage { get; set; } = "";
    /// <summary>
    /// The checksum of this file.
    /// </summary>
    public string Checksum { get; set; } = "";
    
    /* Public methods. */
    public override int IndexOfAttribute(string name)
    {
        switch (name)
        {
            case "id":
                return 0;
            case "editor":
                return 1;
            case "schema_version":
                return 2;
            case "format_version":
                return 3;
            case "default_language":
                return 4;
            case "checksum":
                return 5;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the name '{name}'.");
        }
    }
    
    
    public override string NameOfAttribute(int index)
    {
        switch (index)
        {
            case 0:
                return "id";
            case 1:
                return "editor";
            case 2:
                return "schema_version";
            case 3:
                return "format_version";
            case 4:
                return "default_language";
            case 5:
                return "checksum";
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override Type TypeOfAttribute(int index)
    {
        switch (index)
        {
            case 0:
                return ID.GetType();
            case 1:
                return Editor.GetType();
            case 2:
                return SchemaVersion.GetType();
            case 3:
                return FormatVersion.GetType();
            case 4:
                return DefaultLanguage.GetType();
            case 5:
                return Checksum.GetType();
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
    public override object GetAttribute(int index)
    {
        switch (index)
        {
            case 0:
                return ID;
            case 1:
                return Editor;
            case 2:
                return SchemaVersion;
            case 3:
                return FormatVersion;
            case 4:
                return DefaultLanguage;
            case 5:
                return Checksum;
            default:
                throw new ArgumentOutOfRangeException($"No attribute corresponds to the index {index}.");
        }
    }
    
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
                FormatVersion = (string)value;
                break;
            case 4:
                DefaultLanguage = (string)value;
                break;
            case 5:
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
            case NodeElement:
            case FrameElement:
            case MemoElement:
            case JointElement:
            case EdgeElement:
                Children.Add(child);
                break;
            default:
                throw new ArgumentOutOfRangeException($"Elements of type {GetType().Name} do not allow children of type {child.GetType().Name}.");
        }
    }
}