using Rusty.Cutscenes;
using Rusty.EditorUI;

namespace Rusty.CutsceneEditor
{
	/// <summary>
	/// A cutscene resource inspector.
	/// </summary>
	public abstract partial class Inspector<T> : ElementVBox where T : CutsceneResource
	{
		/* Public properties. */
		/// <summary>
		/// The instruction set used in this inspector.
		/// </summary>
		public InstructionSet InstructionSet { get; private set; }
		/// <summary>
		/// The cutscene resource visualized by this inspector.
		/// </summary>
		public T Resource { get; set; }

        /* Constructors. */
        public Inspector() : base() { }

		public Inspector(InstructionSet instructionSet, T resource) : base()
		{
			InstructionSet = instructionSet;
			Resource = resource;
			Init(resource);
		}

		public Inspector(Inspector<T> other) : this(other.InstructionSet, other.Resource)
		{
			CopyStateFrom(other);
		}

        public override bool CopyStateFrom(Element other)
        {
			if (base.CopyStateFrom(other) && other is Inspector<T> otherInspector)
			{
				InstructionSet = otherInspector.InstructionSet;
				Resource = otherInspector.Resource;
				return true;
			}
			else
				return false;
        }

        /* Protected methods. */
        /// <summary>
        /// Inspector initialization method.
        /// </summary>
        protected virtual void Init(T resource) { }
	}
}