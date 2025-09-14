using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch;

public static class PgSearchFunctionsExtensions
{
    [DbFunction("|||", IsBuiltIn = false)]
    public static bool MatchDisjunction<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchDisjunction)));
    }

    [DbFunction("|||", IsBuiltIn = false)]
    public static bool MatchDisjunction<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Fuzzy fuzzy
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchDisjunction)));
    }

    [DbFunction("&&&", IsBuiltIn = false)]
    public static bool MatchConjunction<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchConjunction)));
    }

    [DbFunction("&&&", IsBuiltIn = false)]
    public static bool MatchConjunction<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Fuzzy fuzzy
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchConjunction)));
    }

    [DbFunction("###", IsBuiltIn = false)]
    public static bool Phrase<TProperty>(this DbFunctions _, TProperty property, string value)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Phrase)));
    }

    [DbFunction("===", IsBuiltIn = false)]
    public static bool Term<TProperty>(this DbFunctions _, TProperty property, string value)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Term)));
    }

    [DbFunction("===", IsBuiltIn = false)]
    public static bool Term<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Fuzzy fuzzy
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Term)));
    }

    [DbFunction("score", "paradedb", IsBuiltIn = false)]
    public static double Score<TProperty>(this DbFunctions _, TProperty property)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Score)));
    }

    [DbFunction("snippet", "paradedb", IsBuiltIn = false)]
    public static string Snippet<TProperty>(this DbFunctions _, TProperty property)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Snippet)));
    }

    [DbFunction("snippet", "paradedb", IsBuiltIn = false)]
    public static string Snippet<TProperty>(this DbFunctions _, TProperty property, int maxNumChars)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Snippet)));
    }

    [DbFunction("snippet", "paradedb", IsBuiltIn = false)]
    public static string Snippet<TProperty>(
        this DbFunctions _,
        TProperty property,
        string startTag,
        string endTag
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Snippet)));
    }

    [DbFunction("snippet", "paradedb", IsBuiltIn = false)]
    public static string Snippet<TProperty>(
        this DbFunctions _,
        TProperty property,
        string startTag,
        string endTag,
        int maxNumChars
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Snippet)));
    }
}
