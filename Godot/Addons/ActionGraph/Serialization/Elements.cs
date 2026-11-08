////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// The ActionGraph element types.
/// </summary>
internal static class Elements
{
    /* Public constants. */
    public const int ELEMENT_COUNT = 26;
    
    /* Public methods. */
    /// <summary>
    /// Get the index of an element, using its name.
    /// </summary>
    public static int IndexOfElement(string name) => name switch
    {
        FileElement.ELEMENT_NAME => FileElement.ELEMENT_INDEX,
        LanguageDefinitionElement.ELEMENT_NAME => LanguageDefinitionElement.ELEMENT_INDEX,
        MetadataElement.ELEMENT_NAME => MetadataElement.ELEMENT_INDEX,
        InstructionDefinitionElement.ELEMENT_NAME => InstructionDefinitionElement.ELEMENT_INDEX,
        ParameterDefinitionElement.ELEMENT_NAME => ParameterDefinitionElement.ELEMENT_INDEX,
        NodeDefinitionElement.ELEMENT_NAME => NodeDefinitionElement.ELEMENT_INDEX,
        FormDefinitionElement.ELEMENT_NAME => FormDefinitionElement.ELEMENT_INDEX,
        OptionDefinitionElement.ELEMENT_NAME => OptionDefinitionElement.ELEMENT_INDEX,
        ChoiceDefinitionElement.ELEMENT_NAME => ChoiceDefinitionElement.ELEMENT_INDEX,
        TupleDefinitionElement.ELEMENT_NAME => TupleDefinitionElement.ELEMENT_INDEX,
        ListDefinitionElement.ELEMENT_NAME => ListDefinitionElement.ELEMENT_INDEX,
        FieldDefinitionElement.ELEMENT_NAME => FieldDefinitionElement.ELEMENT_INDEX,
        PortDefinitionElement.ELEMENT_NAME => PortDefinitionElement.ELEMENT_INDEX,
        NodeElement.ELEMENT_NAME => NodeElement.ELEMENT_INDEX,
        MemoElement.ELEMENT_NAME => MemoElement.ELEMENT_INDEX,
        FrameElement.ELEMENT_NAME => FrameElement.ELEMENT_INDEX,
        JointElement.ELEMENT_NAME => JointElement.ELEMENT_INDEX,
        EdgeElement.ELEMENT_NAME => EdgeElement.ELEMENT_INDEX,
        FormElement.ELEMENT_NAME => FormElement.ELEMENT_INDEX,
        OptionElement.ELEMENT_NAME => OptionElement.ELEMENT_INDEX,
        ChoiceElement.ELEMENT_NAME => ChoiceElement.ELEMENT_INDEX,
        TupleElement.ELEMENT_NAME => TupleElement.ELEMENT_INDEX,
        ListElement.ELEMENT_NAME => ListElement.ELEMENT_INDEX,
        FieldElement.ELEMENT_NAME => FieldElement.ELEMENT_INDEX,
        PortElement.ELEMENT_NAME => PortElement.ELEMENT_INDEX,
        LocalizedValueElement.ELEMENT_NAME => LocalizedValueElement.ELEMENT_INDEX,
        _ => throw new ArgumentException($"Unknown element '{name}'.", nameof(name))
    };

    /// <summary>
    /// Get the name of an element, using its index.
    /// </summary>
    public static string NameOfElement(int index) => index switch
    {
        FileElement.ELEMENT_INDEX => FileElement.ELEMENT_NAME,
        LanguageDefinitionElement.ELEMENT_INDEX => LanguageDefinitionElement.ELEMENT_NAME,
        MetadataElement.ELEMENT_INDEX => MetadataElement.ELEMENT_NAME,
        InstructionDefinitionElement.ELEMENT_INDEX => InstructionDefinitionElement.ELEMENT_NAME,
        ParameterDefinitionElement.ELEMENT_INDEX => ParameterDefinitionElement.ELEMENT_NAME,
        NodeDefinitionElement.ELEMENT_INDEX => NodeDefinitionElement.ELEMENT_NAME,
        FormDefinitionElement.ELEMENT_INDEX => FormDefinitionElement.ELEMENT_NAME,
        OptionDefinitionElement.ELEMENT_INDEX => OptionDefinitionElement.ELEMENT_NAME,
        ChoiceDefinitionElement.ELEMENT_INDEX => ChoiceDefinitionElement.ELEMENT_NAME,
        TupleDefinitionElement.ELEMENT_INDEX => TupleDefinitionElement.ELEMENT_NAME,
        ListDefinitionElement.ELEMENT_INDEX => ListDefinitionElement.ELEMENT_NAME,
        FieldDefinitionElement.ELEMENT_INDEX => FieldDefinitionElement.ELEMENT_NAME,
        PortDefinitionElement.ELEMENT_INDEX => PortDefinitionElement.ELEMENT_NAME,
        NodeElement.ELEMENT_INDEX => NodeElement.ELEMENT_NAME,
        MemoElement.ELEMENT_INDEX => MemoElement.ELEMENT_NAME,
        FrameElement.ELEMENT_INDEX => FrameElement.ELEMENT_NAME,
        JointElement.ELEMENT_INDEX => JointElement.ELEMENT_NAME,
        EdgeElement.ELEMENT_INDEX => EdgeElement.ELEMENT_NAME,
        FormElement.ELEMENT_INDEX => FormElement.ELEMENT_NAME,
        OptionElement.ELEMENT_INDEX => OptionElement.ELEMENT_NAME,
        ChoiceElement.ELEMENT_INDEX => ChoiceElement.ELEMENT_NAME,
        TupleElement.ELEMENT_INDEX => TupleElement.ELEMENT_NAME,
        ListElement.ELEMENT_INDEX => ListElement.ELEMENT_NAME,
        FieldElement.ELEMENT_INDEX => FieldElement.ELEMENT_NAME,
        PortElement.ELEMENT_INDEX => PortElement.ELEMENT_NAME,
        LocalizedValueElement.ELEMENT_INDEX => LocalizedValueElement.ELEMENT_NAME,
        _ => throw new ArgumentException($"Unknown element '{index}'.", nameof(index))
    };

    /// <summary>
    /// Get the type of an element, using its index.
    /// </summary>
    public static Type TypeOfElement(int index) => index switch
    {
        FileElement.ELEMENT_INDEX => typeof(FileElement),
        LanguageDefinitionElement.ELEMENT_INDEX => typeof(LanguageDefinitionElement),
        MetadataElement.ELEMENT_INDEX => typeof(MetadataElement),
        InstructionDefinitionElement.ELEMENT_INDEX => typeof(InstructionDefinitionElement),
        ParameterDefinitionElement.ELEMENT_INDEX => typeof(ParameterDefinitionElement),
        NodeDefinitionElement.ELEMENT_INDEX => typeof(NodeDefinitionElement),
        FormDefinitionElement.ELEMENT_INDEX => typeof(FormDefinitionElement),
        OptionDefinitionElement.ELEMENT_INDEX => typeof(OptionDefinitionElement),
        ChoiceDefinitionElement.ELEMENT_INDEX => typeof(ChoiceDefinitionElement),
        TupleDefinitionElement.ELEMENT_INDEX => typeof(TupleDefinitionElement),
        ListDefinitionElement.ELEMENT_INDEX => typeof(ListDefinitionElement),
        FieldDefinitionElement.ELEMENT_INDEX => typeof(FieldDefinitionElement),
        PortDefinitionElement.ELEMENT_INDEX => typeof(PortDefinitionElement),
        NodeElement.ELEMENT_INDEX => typeof(NodeElement),
        MemoElement.ELEMENT_INDEX => typeof(MemoElement),
        FrameElement.ELEMENT_INDEX => typeof(FrameElement),
        JointElement.ELEMENT_INDEX => typeof(JointElement),
        EdgeElement.ELEMENT_INDEX => typeof(EdgeElement),
        FormElement.ELEMENT_INDEX => typeof(FormElement),
        OptionElement.ELEMENT_INDEX => typeof(OptionElement),
        ChoiceElement.ELEMENT_INDEX => typeof(ChoiceElement),
        TupleElement.ELEMENT_INDEX => typeof(TupleElement),
        ListElement.ELEMENT_INDEX => typeof(ListElement),
        FieldElement.ELEMENT_INDEX => typeof(FieldElement),
        PortElement.ELEMENT_INDEX => typeof(PortElement),
        LocalizedValueElement.ELEMENT_INDEX => typeof(LocalizedValueElement),
        _ => throw new ArgumentException($"Unknown element '{index}'.", nameof(index))
    };
    
    /// <summary>
    /// Instantiate an element of some type, using its index.
    /// </summary>
    public static Element Instantiate(int index) => (Element)Activator.CreateInstance(TypeOfElement(index));
}