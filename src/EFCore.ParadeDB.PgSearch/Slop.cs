namespace EFCore.ParadeDB.PgSearch;

public sealed class Slop
{
    private readonly int _value;

    private Slop(int value)
    {
        _value = value;
    }

    public static Slop With(int value) => new(value);

    public override string ToString() => $"pdb.slop({_value})";
}
