namespace EFCore.ParadeDB.PgSearch.Internal.Modifiers;

public sealed class Boost
{
    private readonly float _factor;

    internal Boost(float factor)
    {
        _factor = factor;
    }

    public override string ToString() => $"pdb.boost({_factor})";
}
