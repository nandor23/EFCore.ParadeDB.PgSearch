using EFCore.PgSearch.Tokenizers;

namespace EFCore.PgSearch;

public static class PgSearch
{
    public static bool Match<TKey, TField>(
        TKey key,
        TField field,
        string value,
        Tokenizer? tokenizer = null,
        int? distance = null,
        bool? transpositionCostOne = null,
        bool? prefix = null,
        bool? conjunctionMode = null
    )
    {
        throw new InvalidOperationException("This method is for use in LINQ queries only");
    }
}
