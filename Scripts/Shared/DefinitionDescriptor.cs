using System.Collections.Generic;

namespace Rusty.ISA.Editor
{
    public class ParameterDescriptor
    {
        public string Type { get; set; }
        public string ID { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        // Non-output only.
        public string DefaultValue { get; set; }

        // Slider-only.
        public string MinValue { get; set; }
        public string MaxValue { get; set; }

        // Output-only.
        public string RemoveDefaultOutput { get; set; }
        public string PreviewArgument { get; set; }
    }

    /// <summary>
    /// An instruction definition descriptor.
    /// </summary>
    public class DefinitionDescriptor
    {
        // Definition.
        public string opcode = "";
        public List<Parameter> parameters = new();
        public Implementation implementation = null;

        // Meta-data.
        public string iconPath = "";
        public string displayName = "";
        public string description = "";
        public string category = "";

        // Editor.
        public EditorNodeInfo editorNodeInfo = null;
        public List<PreviewTerm> previewTerms = new();
        public List<CompileRule> preInstructions = new();
        public List<CompileRule> postInstructions = new();
    }
}