namespace EFCore.ParadeDB.PgSearch;

public sealed class Fuzzy
{
    private readonly int _distance;
    private readonly bool _prefix;
    private readonly bool _transpositionCostOne;

    private Fuzzy(int distance, bool prefix = false, bool transpositionCostOne = false)
    {
        _distance = distance;
        _prefix = prefix;
        _transpositionCostOne = transpositionCostOne;
    }

    public static Fuzzy With(int distance) => new(distance);

    public static Fuzzy With(int distance, bool prefix) => new(distance, prefix);

    public static Fuzzy With(int distance, bool prefix, bool transpositionCostOne) =>
        new(distance, prefix, transpositionCostOne);

    public override string ToString()
    {
        return (_prefix, _transpositionCostOne) switch
        {
            (false, false) => $"fuzzy({_distance})",
            (true, false) => $"fuzzy({_distance}, t)",
            (false, true) => $"fuzzy({_distance}, f, t)",
            (true, true) => $"fuzzy({_distance}, t, t)",
        };
    }
}
