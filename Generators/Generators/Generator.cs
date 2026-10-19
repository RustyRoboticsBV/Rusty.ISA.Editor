using System.Text;

namespace CodecGenerator.Generators
{
    internal abstract class Generator
    {
        public abstract void Generate(StringBuilder sb);
    }
}
