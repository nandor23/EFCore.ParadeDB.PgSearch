namespace EFCore.ParadeDB.PgSearch;

public sealed class Boost
{
    private readonly float _factor;

    private Boost(float factor)
    {
        _factor = factor;
    }

    public static Boost With(int factor) => new(factor);

    public override string ToString()
    {
        return $"boost({_factor})";
    }
}
