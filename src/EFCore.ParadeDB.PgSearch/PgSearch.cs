using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch;

public static class PgSearch
{
    [DbFunction("|||", IsBuiltIn = false)]
    public static bool MatchDisjunction<TProperty>(TProperty property, string value)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchDisjunction)));
    }

    [DbFunction("|||", IsBuiltIn = false)]
    public static bool MatchDisjunction<TProperty>(
        TProperty property,
        string value,
        [NotParameterized] Fuzzy fuzzy
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchDisjunction)));
    }

    [DbFunction("&&&", IsBuiltIn = false)]
    public static bool MatchConjunction<TProperty>(TProperty property, string value)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchConjunction)));
    }

    [DbFunction("&&&", IsBuiltIn = false)]
    public static bool MatchConjunction<TProperty>(
        TProperty property,
        string value,
        [NotParameterized] Fuzzy fuzzy
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchConjunction)));
    }

    [DbFunction("###", IsBuiltIn = false)]
    public static bool Phrase<TProperty>(TProperty property, string value)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Phrase)));
    }

    [DbFunction("===", IsBuiltIn = false)]
    public static bool Term<TProperty>(TProperty property, string value)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Term)));
    }

    [DbFunction("===", IsBuiltIn = false)]
    public static bool Term<TProperty>(
        TProperty property,
        string value,
        [NotParameterized] Fuzzy fuzzy
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Term)));
    }

    [DbFunction("score", "paradedb", IsBuiltIn = false)]
    public static double Score<TProperty>(TProperty property)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Score)));
    }

    [DbFunction("snippet", "paradedb", IsBuiltIn = false)]
    public static string Snippet<TProperty>(TProperty property)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Snippet)));
    }

    [DbFunction("snippet", "paradedb", IsBuiltIn = false)]
    public static string Snippet<TProperty>(TProperty property, int maxNumChars)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Snippet)));
    }

    [DbFunction("snippet", "paradedb", IsBuiltIn = false)]
    public static string Snippet<TProperty>(TProperty property, string startTag, string endTag)
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Snippet)));
    }

    [DbFunction("snippet", "paradedb", IsBuiltIn = false)]
    public static string Snippet<TProperty>(
        TProperty property,
        string startTag,
        string endTag,
        int maxNumChars
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Snippet)));
    }
}
