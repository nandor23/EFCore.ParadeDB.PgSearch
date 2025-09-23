namespace EFCore.ParadeDB.PgSearch;

public sealed class Boost
{
    internal readonly float Factor;

    private Boost(float factor)
    {
        Factor = factor;
    }

    public static Boost With(float factor) => new(factor);

    public override string ToString()
    {
        return $"boost({Factor})";
    }
}
