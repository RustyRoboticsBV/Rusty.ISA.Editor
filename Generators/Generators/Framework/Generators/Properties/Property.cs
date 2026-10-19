using System;

/// <summary>
/// A property generator.
/// </summary>
public sealed class Property : Generator
{
    public Summary Summary { get; set; }
    public Access Access { get; set; } = AccessType.Public;
    public Modifier Modifier { get; set; } = ModifierType.None;
    public Id Type { get; set; }
    public Id Name { get; set; }
    public Getter Getter { get; set; }
    public Setter Setter { get; set; }

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

        bool lambda = !(Getter != null && Setter != null) && (Getter?.SingleLine == true || Setter?.SingleLine == true);
        bool block = Getter != null && Setter != null && Getter.Implementation != null && Setter.Implementation != null
            || Getter?.SingleLine == false || Setter?.SingleLine == false;

        if (block)
        {
            w.OpenBlock();
            w.Line();
        }
        else if (!lambda)
            w.Append(" { ");

        if (Getter != null && Setter != null)
        {
            Getter.Generate(w);
            if (block)
                w.Line();
            else
                w.Space();
            Setter.Generate(w);
        }
        else if (Getter != null)
            WriteSingular(Getter, lambda, w);
        else if (Setter != null)
            WriteSingular(Setter, lambda, w);
        else
            throw new InvalidOperationException("A property must have a getter or setter.");

        if (block)
            w.CloseBlock();
        else if (!lambda)
            w.Append(" }");

        return true;
    }

    public Property() { }

    public Property(Access access, Modifier modifier, Id type, Id name)
    {
        Access = access;
        Modifier = modifier;
        Type = type;
        Name = name;
    }

    public Property AddSummary(Summary summary)
    {
        Summary = summary;
        return this;
    }

    public Property AddGetter(Getter getter)
    {
        Getter = getter;
        return this;
    }

    public Property AddSetter(Setter setter)
    {
        Setter = setter;
        return this;
    }

    private static void WriteSingular(Accessor accessor, bool lambda, CodeWriter w)
    {
        if (lambda)
        {
            w.Append(" => ");
            if (accessor.Implementation is Return @return)
                @return.GenerateImplicit(w);
            else
                accessor.Implementation.Generate(w);
        }
        else
            accessor.Generate(w);
    }
}