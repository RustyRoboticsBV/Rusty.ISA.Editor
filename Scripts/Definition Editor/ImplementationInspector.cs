using Godot;
using System;
using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Definitions
{
    /// <summary>
    /// A generic parameter definition inspector.
    /// </summary>
    public partial class ImplementationInspector : Element
    {
        /* Public methods. */
        public CheckBoxFoldout Foldout { get; private set; }
        public ListElement Dependencies { get; private set; }
        public MultilineField Members { get; private set; }
        public MultilineField Initialize { get; private set; }
        public MultilineField Execute { get; private set; }

        public ImplementationDescriptor Value
        {
            get
            {
                if (Foldout.IsOpen)
                {
                    string[] deps = new string[Dependencies.Count];
                    for (int i = 0; i < deps.Length; i++)
                    {
                        deps[i] = (Dependencies[i].GetAt(0) as LineField).Value;
                    }
                    return new(deps, Members.Value, Initialize.Value, Execute.Value);
                }
                else
                    return null;
            }
            set
            {
                Foldout.IsOpen = value != null;
                if (Foldout.IsOpen)
                {
                    for (int i = 0; i < value.Dependencies.Count; i++)
                    {
                        Dependencies.Add();
                        (Dependencies[i].GetAt(0) as LineField).Value = value.Dependencies[i];
                    }
                    Members.Value = value.Members;
                    Initialize.Value = value.Initialize;
                    Execute.Value = value.Execute;
                }
            }
        }

        /* Private properties. */
        private string LastFilePath { get; set; }

        /* Constructors. */
        public ImplementationInspector() : base() { }

        public ImplementationInspector(ImplementationInspector other) : base(other) { }

        /* Public methods. */
        public override Element Duplicate()
        {
            return new ImplementationInspector(this);
        }

        public override bool CopyStateFrom(Element other)
        {
            if (base.CopyStateFrom(other) && other is ImplementationInspector otherParameter)
            {


                return true;
            }
            else
                return false;
        }

        /* Protected methods. */
        protected override void Init()
        {
            base.Init();

            Name = "ImplementationInspector";

            Foldout = new();
            Foldout.HeaderText = "Has Implementation?";
            AddChild(Foldout);

            Dependencies = new();
            Foldout.Add(Dependencies);
            Dependencies.Name = "Dependencies";
            Dependencies.HeaderText = "Dependencies";
            Dependencies.Template = new LineField() { LabelText = "Class Name" };

            Members = new();
            Foldout.Add(Members);
            Members.Name = "Members";
            Members.LabelText = "Members";
            Members.Height = 128f;

            Initialize = new();
            Foldout.Add(Initialize);
            Initialize.Name = "Initialize";
            Initialize.LabelText = "Initialize";
            Initialize.Height = 128f;

            Execute = new();
            Foldout.Add(Execute);
            Execute.Name = "Execute";
            Execute.LabelText = "Execute";
            Execute.Height = 128f;
        }
    }
}