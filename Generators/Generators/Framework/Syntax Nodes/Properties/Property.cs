using System;

/// <summary>
/// A property generator.
/// </summary>
public sealed class Property : SyntaxNode, IClassMember
{
    /* Public properties. */
    public override bool SingleLine => Summary == null && OneLine;

    public Summary Summary { get; set; }
    public Access Access { get; set; } = AccessType.Public;
    public Modifier Modifier { get; set; } = ModifierType.None;
    public TypeName Type { get; set; }
    public Id Name { get; set; }
    public Getter Getter { get; set; }
    public Setter Setter { get; set; }
    public Value Initialization { get; set; }

    /* Private properties. */
    bool Lambda =>
        (Getter != null && !Getter.Implementation.IsEmpty && Getter.SingleLine && (Setter == null || Setter.Implementation.IsEmpty))
        || (Setter != null && !Setter.Implementation.IsEmpty && Setter.SingleLine && (Getter == null || Getter.Implementation.IsEmpty));
    bool OneLine => Lambda
        || ((Getter == null || Getter.Implementation.IsEmpty) && (Setter == null || Setter.Implementation.IsEmpty));

    /* Public methods. */
    public override bool Generate(CodeWriter w)
    {
        if (Summary != null)
        {
            Summary.Generate(w);
            w.Line();
        }

        if (Access.Generate(w))
            w.Space();
        if (Modifier.Generate(w))
            w.Space();
        Type.Generate(w);
        w.Space();
        Name.Generate(w);

        if (!Lambda && !OneLine)
        {
            w.OpenBlock();
            w.Line();
        }
        else if (!Lambda && OneLine)
            w.Append(" { ");

        if (Getter != null && Setter != null)
        {
            Getter.Generate(w);
            if (!OneLine)
                w.Line();
            else
                w.Space();
            Setter.Generate(w);
        }
        else if (Getter != null)
            WriteSingular(Getter, Lambda, w);
        else if (Setter != null)
            WriteSingular(Setter, Lambda, w);
        else
            throw new InvalidOperationException("A property must have a getter or setter.");

        if (!Lambda && !OneLine)
            w.CloseBlock();
        else if (!Lambda && OneLine)
            w.Append(" }");

        if (Initialization != null)
        {
            w.Append(" = ");
            Initialization.Generate(w);
            w.Append(';');
        }

        return true;
    }

    private static void WriteSingular(Accessor accessor, bool lambda, CodeWriter w)
    {
        if (lambda)
        {
            w.Append(" => ");
            if (accessor.Implementation.Statements.Count == 1 && accessor.Implementation.Statements[0] is Return @return)
                @return.GenerateImplicit(w);
            else
                accessor.Implementation.Generate(w);
        }
        else
            accessor.Generate(w);
    }
}