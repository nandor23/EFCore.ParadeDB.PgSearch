namespace EFCore.ParadeDB.PgSearch.Internal.Modifiers;

public sealed class Slop
{
    private readonly int _value;

    internal Slop(int value)
    {
        _value = value;
    }

    public override string ToString() => $"pdb.slop({_value})";
}
