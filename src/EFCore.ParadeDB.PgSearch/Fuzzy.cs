namespace EFCore.ParadeDB.PgSearch;

public sealed class Fuzzy
{
    private readonly int _distance;
    private readonly bool _isPrefix;
    private readonly bool _hasLowTranspositionCost;

    private Fuzzy(int distance, bool isPrefix, bool hasLowTranspositionCost)
    {
        _distance = distance;
        _isPrefix = isPrefix;
        _hasLowTranspositionCost = hasLowTranspositionCost;
    }

    public static Fuzzy With(
        int distance,
        bool isPrefix = false,
        bool lowerTranspositionCost = false
    ) => new(distance, isPrefix, lowerTranspositionCost);

    internal string ToSql()
    {
        return (_isPrefix, _hasLowTranspositionCost) switch
        {
            (false, false) => $"fuzzy({_distance})",
            (true, false) => $"fuzzy({_distance}, t)",
            (false, true) => $"fuzzy({_distance}, f, t)",
            (true, true) => $"fuzzy({_distance}, t, t)",
        };
    }
}
