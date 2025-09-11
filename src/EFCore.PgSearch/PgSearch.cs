using EFCore.PgSearch.Tokenizers;

namespace EFCore.PgSearch;

public static class PgSearch
{
    public static bool MatchDisjunction<TField>(TField field, string value)
    {
        throw new InvalidOperationException("This method is for use in LINQ queries only");
    }

    public static bool MatchConjunction<TField>(TField field, string value)
    {
        throw new InvalidOperationException("This method is for use in LINQ queries only");
    }

    public static bool Phrase<TField>(TField field, string value)
    {
        throw new InvalidOperationException("This method is for use in LINQ queries only");
    }

    public static bool Term<TField>(TField field, string value)
    {
        throw new InvalidOperationException("This method is for use in LINQ queries only");
    }

    public static bool Match<TField>(
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
