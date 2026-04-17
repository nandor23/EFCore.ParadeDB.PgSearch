using EFCore.ParadeDB.PgSearch.Internal.Modifiers;

namespace EFCore.ParadeDB.PgSearch;

public static class Pdb
{
    public static Boost Boost(float factor) => new(factor);

    public static Fuzzy Fuzzy(
        int distance,
        bool prefix = false,
        bool transpositionCostOne = false
    ) => new(distance, prefix, transpositionCostOne);

    public static Slop Slop(int value) => new(value);
}
