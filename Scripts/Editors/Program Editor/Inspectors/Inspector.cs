using Rusty.EditorUI;

namespace Rusty.ISA.Editor.Programs
{
	/// <summary>
	/// A ISA resource inspector.
	/// </summary>
	public abstract partial class Inspector : ElementVBox
	{
		/* Public properties. */
		/// <summary>
		/// The instruction set used in this inspector.
		/// </summary>
		public InstructionSet InstructionSet { get; private set; }
		/// <summary>
		/// The ISA resource visualized by this inspector.
		/// </summary>
		public InstructionResource Resource { get; set; }

        /* Constructors. */
        public Inspector() : base() { }

		public Inspector(InstructionSet instructionSet, InstructionResource resource) : base()
		{
			InstructionSet = instructionSet;
			Resource = resource;
			Init();
		}

		public Inspector(Inspector other) : this(other.InstructionSet, other.Resource)
		{
			CopyStateFrom(other);
		}

		/* Public methods. */
        public override bool CopyStateFrom(Element other)
		{
			if (base.CopyStateFrom(other) && other is Inspector otherInspector)
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
		protected new virtual void Init() { }
	}
}