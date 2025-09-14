using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch;

public static class PgSearch
{
    [DbFunction("|||", IsBuiltIn = false)]
    public static bool MatchDisjunction<TField>(TField field, string value)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchDisjunction)));
    }

    [DbFunction("|||", IsBuiltIn = false)]
    public static bool MatchDisjunction<TField>(
        TField field,
        string value,
        [NotParameterized] Fuzzy fuzzy
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchDisjunction)));
    }

    [DbFunction("&&&", IsBuiltIn = false)]
    public static bool MatchConjunction<TField>(TField field, string value)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchConjunction)));
    }

    [DbFunction("&&&", IsBuiltIn = false)]
    public static bool MatchConjunction<TField>(
        TField field,
        string value,
        [NotParameterized] Fuzzy fuzzy
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchConjunction)));
    }

    [DbFunction("###", IsBuiltIn = false)]
    public static bool Phrase<TField>(TField field, string value)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Phrase)));
    }

    [DbFunction("===", IsBuiltIn = false)]
    public static bool Term<TField>(TField field, string value)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Term)));
    }

    [DbFunction("===", IsBuiltIn = false)]
    public static bool Term<TField>(TField field, string value, [NotParameterized] Fuzzy fuzzy)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Term)));
    }

    [DbFunction("score", "paradedb", IsBuiltIn = false)]
    public static double Score<TKey>(TKey key)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Score)));
    }
}
