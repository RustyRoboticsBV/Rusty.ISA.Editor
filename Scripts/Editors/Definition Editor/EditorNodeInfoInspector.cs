using Rusty.EditorUI;
using Godot;

namespace Rusty.ISA.Editor.Definitions
{
    /// <summary>
    /// An inspector for an editor node info object.
    /// </summary>
    public partial class EditorNodeInfoInspector : ElementVBox
    {
        /* Public properties. */
        public CheckBoxFoldout Foldout { get; private set; }

        public IntField Priority { get; private set; }
        public IntField MinWidth { get; private set; }
        public IntField MinHeight { get; private set; }
        public ColorField MainColor { get; private set; }
        public ColorField TextColor { get; private set; }
        public MultilineField Preview { get; private set; }
        public CheckBoxField EnableWordWrap { get; private set; }

        public EditorNodeInfoDescriptor Value
        {
            get
            {
                if (Foldout.IsOpen)
                    return new(Priority.Value, MinWidth.Value, MinHeight.Value, MainColor.Value, TextColor.Value, Preview.Value, EnableWordWrap.Value);
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    Foldout.IsOpen = true;
                    Priority.Value = value.Priority;
                    MinWidth.Value = value.MinWidth;
                    MinHeight.Value = value.MinHeight;
                    MainColor.Value = value.MainColor;
                    TextColor.Value = value.TextColor;
                    Preview.Value = value.Preview;
                    EnableWordWrap.Value = value.EnableWordWrap;
                }
                else
                    Foldout.IsOpen = false;
            }
        }

        /* Constructors. */
        public EditorNodeInfoInspector() : base() { }

        /* Protected methods. */
        protected override void Init()
        {
            base.Init();

            Foldout = new();
            Add(Foldout);
            Foldout.HeaderText = "Has Editor Node";

            Priority = new();
            Foldout.Add(Priority);
            Priority.LabelText = "Priority";

            MinWidth = new()
            {
                LabelText = "Min Width",
                Value = 128
            };
            Foldout.Add(MinWidth);

            MinHeight = new()
            {
                LabelText = "Min Height",
                Value = 32
            };
            Foldout.Add(MinHeight);

            MainColor = new()
            {
                LabelText = "Main Color",
                Value = Color.FromHtml("696969")
            };
            Foldout.Add(MainColor);

            TextColor = new();
            Foldout.Add(TextColor);
            TextColor.LabelText = "Text Color";
            TextColor.Value = Colors.White;

            Preview = new();
            Foldout.Add(Preview);
            Preview.LabelText = "Preview";
            Preview.Height = 256;

            EnableWordWrap = new();
            Foldout.Add(EnableWordWrap);
            EnableWordWrap.LabelText = "Enable Word Wrap";
        }
    }
}