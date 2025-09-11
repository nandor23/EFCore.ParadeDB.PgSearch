using EFCore.PgSearch.Tokenizers;

namespace EFCore.PgSearch;

public static class PgSearch
{
    public static bool Match<TKey, TField>(
        TKey key,
        TField field,
        string value,
        Tokenizer? tokenizer = null
    )
    {
        throw new InvalidOperationException("This method is for use in LINQ queries only");
    }
}
