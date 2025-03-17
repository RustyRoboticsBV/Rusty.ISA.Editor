﻿using Godot;
using System;
using System.Collections.Generic;
using Rusty.Xml;
using Rusty.ISA;

namespace Rusty.ISA.Importer.InstructionDefinitions
{
    /// <summary>
    /// An deserializer for XML-based ISA instruction definitions.
    /// </summary>
    public static class Deserializer
    {
        /* Public methods. */
        /// <summary>
        /// Decompile a string of XML into an instruction definition.
        /// </summary>
        public static InstructionDefinition Deserialize(string xml, string iconFolderPath)
        {
            // Load XML document.
            Document document = new Document("definition", xml);

            // Init constructor arguments.
            ConstructorArgs args = new ConstructorArgs();

            // Find parameters, preview terms & compile rules.
            for (int i = 0; i < document.Root.Children.Count; i++)
            {
                Element element = document.Root.Children[i];

                switch (element.Name)
                {
                    case Keywords.Opcode:
                        args.opcode = element.InnerText;
                        break;

                    case Keywords.BoolParameter:
                        args.parameters.Add(new BoolParameter(GetId(element),
                            GetStringChild(element, Keywords.DisplayName),
                            GetStringChild(element, Keywords.Description),
                            GetBoolChild(element, Keywords.DefaultValue)
                        ));
                        break;
                    case Keywords.IntParameter:
                        args.parameters.Add(new IntParameter(GetId(element),
                            GetStringChild(element, Keywords.DisplayName),
                            GetStringChild(element, Keywords.Description),
                            GetIntChild(element, Keywords.DefaultValue)
                        ));
                        break;
                    case Keywords.IntSliderParameter:
                        args.parameters.Add(new IntSliderParameter(GetId(element),
                            GetStringChild(element, Keywords.DisplayName),
                            GetStringChild(element, Keywords.Description),
                            GetIntChild(element, Keywords.DefaultValue),
                            GetIntChild(element, Keywords.MinValue),
                            GetIntChild(element, Keywords.MaxValue)
                        ));
                        break;
                    case Keywords.FloatParameter:
                        args.parameters.Add(new FloatParameter(GetId(element),
                            GetStringChild(element, Keywords.DisplayName),
                            GetStringChild(element, Keywords.Description),
                            GetFloatChild(element, Keywords.DefaultValue)
                        ));
                        break;
                    case Keywords.FloatSliderParameter:
                        args.parameters.Add(new FloatSliderParameter(GetId(element),
                            GetStringChild(element, Keywords.DisplayName),
                            GetStringChild(element, Keywords.Description),
                            GetFloatChild(element, Keywords.DefaultValue),
                            GetFloatChild(element, Keywords.MinValue),
                            GetFloatChild(element, Keywords.MaxValue)
                        ));
                        break;
                    case Keywords.CharParameter:
                        args.parameters.Add(new CharParameter(GetId(element),
                            GetStringChild(element, Keywords.DisplayName),
                            GetStringChild(element, Keywords.Description),
                            GetCharChild(element, Keywords.DefaultValue)
                        ));
                        break;
                    case Keywords.TextParameter:
                        args.parameters.Add(new TextParameter(GetId(element),
                            GetStringChild(element, Keywords.DisplayName),
                            GetStringChild(element, Keywords.Description),
                            GetStringChild(element, Keywords.DefaultValue)
                        ));
                        break;
                    case Keywords.MultilineParameter:
                        args.parameters.Add(new MultilineParameter(GetId(element),
                            GetStringChild(element, Keywords.DisplayName),
                            GetStringChild(element, Keywords.Description),
                            GetStringChild(element, Keywords.DefaultValue)
                        ));
                        break;
                    case Keywords.ColorParameter:
                        args.parameters.Add(new ColorParameter(GetId(element),
                            GetStringChild(element, Keywords.DisplayName),
                            GetStringChild(element, Keywords.Description),
                            GetColorChild(element, Keywords.DefaultValue)
                        ));
                        break;
                    case Keywords.OutputParameter:
                        args.parameters.Add(new OutputParameter(GetId(element),
                            GetStringChild(element, Keywords.DisplayName),
                            GetStringChild(element, Keywords.Description),
                            element.HasChild(Keywords.RemoveDefaultOutput),
                            GetStringChild(element, Keywords.UseArgumentAsPreview)
                        ));
                        break;

                    case Keywords.Implementation:
                        args.implementation = ParseImplementation(element);
                        break;

                    case Keywords.Icon:
                        args.icon = GetTexture(iconFolderPath, element.InnerText);
                        break;
                    case Keywords.DisplayName:
                        args.displayName = element.InnerText;
                        break;
                    case Keywords.Description:
                        args.description = element.InnerText;
                        break;
                    case Keywords.Category:
                        args.category = element.InnerText;
                        break;

                    case Keywords.EditorNodeInfo:
                        EditorNodeInfo defaults = new EditorNodeInfo();
                        int priority = GetIntChild(element, Keywords.Priority, defaults.Priority);
                        int minWidth = GetIntChild(element, Keywords.MinWidth, defaults.MinWidth);
                        int minHeight = GetIntChild(element, Keywords.MinHeight, defaults.MinHeight);
                        Color mainColor = GetColorChild(element, Keywords.MainColor, defaults.MainColor);
                        Color textColor = GetColorChild(element, Keywords.TextColor, defaults.TextColor);
                        args.editorNodeInfo = new EditorNodeInfo(priority, minWidth, minHeight, mainColor, textColor);
                        break;

                    case Keywords.TextTerm:
                    case Keywords.ArgumentTerm:
                    case Keywords.CompileRuleTerm:
                        break;

                    case Keywords.PreInstructions:
                        args.preInstructions = ParseSecondaryInstructions(element);
                        break;
                    case Keywords.PostInstructions:
                        args.postInstructions = ParseSecondaryInstructions(element);
                        break;

                    default:
                        throw new Exception($"Tried to parse unrecognized XML element with name '{element.Name}'.");
                }
            }

            // Create instruction definition.
            return new InstructionDefinition(
                args.opcode, args.parameters.ToArray(), args.implementation,
                args.icon, args.displayName, args.description, args.category,
                args.editorNodeInfo, args.previewTerms.ToArray(), args.preInstructions.ToArray(), args.postInstructions.ToArray()
            );
        }

        /* Private methods. */
        private static string GetId(Element element, string defaultValue = "")
        {
            try
            {
                return element.GetAttribute(Keywords.ID);
            }
            catch
            {
                return defaultValue;
            }
        }

        private static bool GetBoolChild(Element element, string name, bool defaultValue = default)
        {
            try
            {
                return bool.Parse(element.GetChild(name).InnerText);
            }
            catch
            {
                return defaultValue;
            }
        }

        private static int GetIntChild(Element element, string name, int defaultValue = default)
        {
            try
            {
                return int.Parse(element.GetChild(name).InnerText);
            }
            catch
            {
                return defaultValue;
            }
        }

        private static float GetFloatChild(Element element, string name, float defaultValue = default)
        {
            try
            {
                return float.Parse(element.GetChild(name).InnerText);
            }
            catch
            {
                return defaultValue;
            }
        }

        private static char GetCharChild(Element element, string name, char defaultValue = '0')
        {
            try
            {
                return element.GetChild(name).InnerText[0];
            }
            catch
            {
                return defaultValue;
            }
        }

        private static string GetStringChild(Element element, string name, string defaultValue = "")
        {
            try
            {
                return element.GetChild(name).InnerText;
            }
            catch
            {
                return defaultValue;
            }
        }

        private static Color GetColorChild(Element element, string name, Color defaultValue = default)
        {
            string color = element.GetChild(name).InnerText;
            try
            {
                return Color.FromHtml(color);
            }
            catch
            {
                try
                {
                    return ColorNameParser.Parse(color);
                }
                catch
                {
                    return defaultValue;
                }
            }
        }

        private static Texture2D GetTexture(string folderPath, string localFilePath)
        {
            if (folderPath == "")
                return null;

            try
            {
                // Globalize path.
                string globalPath = folderPath + "\\" + localFilePath;

                // Load image.
                Image image = new();
                image.Load(globalPath);

                if (!image.IsEmpty())
                {
                    // Create texture.
                    ImageTexture texture = ImageTexture.CreateFromImage(image);
                    if (!texture.ResourcePath.StartsWith("res://") || !texture.ResourcePath.StartsWith("user://"))
                        texture.ResourcePath = globalPath;
                    return texture;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }


        private static Implementation ParseImplementation(Element element)
        {
            string[] dependencies = new string[0];
            string members = "";
            string initialize = "";
            string execute = "";
            foreach (Element child in element.Children)
            {
                switch (child.Name)
                {
                    case Keywords.Dependencies:
                        dependencies = child.InnerText.Split(',');
                        break;
                    case Keywords.Members:
                        members = child.InnerText;
                        break;
                    case Keywords.Initialize:
                        initialize = child.InnerText;
                        break;
                    case Keywords.Execute:
                        execute = child.InnerText;
                        break;
                }
            }

            return new(dependencies, members, initialize, execute);
        }

        private static List<CompileRule> ParseSecondaryInstructions(Element element)
        {
            List<CompileRule> result = new();

            foreach (Element child in element.Children)
            {
                switch (child.Name)
                {
                    case Keywords.InstructionRule:
                        result.Add(ParseInstruction(child));
                        break;
                    case Keywords.OptionRule:
                        result.Add(ParseOption(child));
                        break;
                    case Keywords.ChoiceRule:
                        result.Add(ParseChoice(child));
                        break;
                    case Keywords.TupleRule:
                        result.Add(ParseTuple(child));
                        break;
                    case Keywords.ListRule:
                        result.Add(ParseList(child));
                        break;
                }
            }

            return result;
        }

        private static CompileRule ParseCompileRule(Element element)
        {
            switch (element.Name)
            {
                case Keywords.DisplayName:
                case Keywords.Description:
                case Keywords.StartEnabled:
                case Keywords.StartSelected:
                case Keywords.AddButtonText:
                case Keywords.PreviewSeparator:
                    return null;
                case Keywords.OptionRule:
                    return ParseOption(element);
                case Keywords.ChoiceRule:
                    return ParseChoice(element);
                case Keywords.TupleRule:
                    return ParseTuple(element);
                case Keywords.ListRule:
                    return ParseList(element);
                case Keywords.InstructionRule:
                    return ParseInstruction(element);
                default:
                    throw new Exception($"Tried to parse XML element '{element.Name}' as a compile rule, but the name does not "
                        + "represent a compile rule.");
            }
        }

        private static InstructionRule ParseInstruction(Element element)
        {
            return new InstructionRule(GetId(element),
                GetStringChild(element, Keywords.DisplayName),
                GetStringChild(element, Keywords.Description),
                GetStringChild(element, Keywords.Opcode)
            );
        }

        private static OptionRule ParseOption(Element element)
        {
            CompileRule target = null;
            for (int i = 0; i < element.Children.Count; i++)
            {
                Element child = element.Children[i];
                CompileRule parsed = ParseCompileRule(child);
                if (parsed != null)
                    target = parsed;
            }

            return new OptionRule(GetId(element),
                GetStringChild(element, Keywords.DisplayName),
                GetStringChild(element, Keywords.Description),
                target,
                GetBoolChild(element, Keywords.StartEnabled)
            );
        }

        private static ChoiceRule ParseChoice(Element element)
        {
            List<CompileRule> targets = new List<CompileRule>();
            for (int i = 0; i < element.Children.Count; i++)
            {
                Element child = element.Children[i];
                CompileRule parsed = ParseCompileRule(child);
                if (parsed != null)
                    targets.Add(parsed);
            }

            return new ChoiceRule(GetId(element),
                GetStringChild(element, Keywords.DisplayName),
                GetStringChild(element, Keywords.Description),
                targets.ToArray(),
                GetIntChild(element, Keywords.StartSelected)
            );
        }

        private static TupleRule ParseTuple(Element element)
        {
            List<CompileRule> targets = new List<CompileRule>();
            for (int i = 0; i < element.Children.Count; i++)
            {
                Element child = element.Children[i];
                CompileRule parsed = ParseCompileRule(child);
                if (parsed != null)
                    targets.Add(parsed);
            }

            return new TupleRule(GetId(element),
                GetStringChild(element, Keywords.DisplayName),
                GetStringChild(element, Keywords.Description),
                targets.ToArray(),
                GetStringChild(element, Keywords.PreviewSeparator)
            );
        }

        private static ListRule ParseList(Element element)
        {
            CompileRule target = null;
            for (int i = 0; i < element.Children.Count; i++)
            {
                Element child = element.Children[i];
                CompileRule parsed = ParseCompileRule(child);
                if (parsed != null)
                    target = parsed;
            }

            return new ListRule(GetId(element),
                GetStringChild(element, Keywords.DisplayName),
                GetStringChild(element, Keywords.Description),
                target,
                GetStringChild(element, Keywords.AddButtonText),
                GetStringChild(element, Keywords.PreviewSeparator)
            );
        }
    }
}