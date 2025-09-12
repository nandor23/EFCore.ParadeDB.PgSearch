using EFCore.ParadeDB.PgSearch.Tokenizers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch;

public static class PgSearch
{
    [DbFunction("|||", IsBuiltIn = false)]
    public static bool MatchDisjunction<TField>(TField field, string value)
    {
        throw new InvalidOperationException("This method is for use in LINQ queries only");
    }

    [DbFunction("|||", IsBuiltIn = false)]
    public static bool MatchDisjunction<TField>(
        TField field,
        string value,
        [NotParameterized] Fuzzy fuzzy
    )
    {
        throw new InvalidOperationException("This method is for use in LINQ queries only");
    }

    public static bool MatchConjunction<TField>(TField field, string value)
    {
        throw new InvalidOperationException("This method is for use in LINQ queries only");
    }

    public static bool MatchConjunction<TField>(
        TField field,
        string value,
        [NotParameterized] Fuzzy fuzzy
    )
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

    public static bool Term<TField>(TField field, string value, [NotParameterized] Fuzzy fuzzy)
    {
        throw new InvalidOperationException("This method is for use in LINQ queries only");
    }
}
