namespace EFCore.ParadeDB.PgSearch;

public sealed class Fuzzy
{
    internal readonly int Distance;
    internal readonly bool Prefix;
    internal readonly bool TranspositionCostOne;

    private Fuzzy(int distance, bool prefix, bool transpositionCostOne)
    {
        Distance = distance;
        Prefix = prefix;
        TranspositionCostOne = transpositionCostOne;
    }

    public static Fuzzy With(
        int distance,
        bool prefix = false,
        bool transpositionCostOne = false
    ) => new(distance, prefix, transpositionCostOne);

    internal string ToSql()
    {
        return (_prefix: Prefix, _transpositionCostOne: TranspositionCostOne) switch
        {
            (false, false) => $"fuzzy({Distance})",
            (true, false) => $"fuzzy({Distance}, t)",
            (false, true) => $"fuzzy({Distance}, f, t)",
            (true, true) => $"fuzzy({Distance}, t, t)",
        };
    }
}
