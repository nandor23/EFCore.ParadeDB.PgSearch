using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch;

public static class PgSearchFunctionsExtensions
{
    public static bool MatchDisjunction<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value
    ) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchDisjunction)));

    public static bool MatchDisjunction<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Fuzzy fuzzy
    ) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchDisjunction)));

    public static bool MatchDisjunction<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Boost boost
    ) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchDisjunction)));

    public static bool MatchDisjunction<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Fuzzy fuzzy,
        [NotParameterized] Boost boost
    ) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchDisjunction)));

    public static bool MatchConjunction<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value
    ) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchConjunction)));

    public static bool MatchConjunction<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Fuzzy fuzzy
    ) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchConjunction)));

    public static bool MatchConjunction<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Boost boost
    ) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchConjunction)));

    public static bool MatchConjunction<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Fuzzy fuzzy,
        [NotParameterized] Boost boost
    ) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(MatchConjunction)));

    public static bool Phrase<TProperty>(this DbFunctions _, TProperty property, string value) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Phrase)));

    public static bool Phrase<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Boost boost
    ) => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Phrase)));

    public static bool Term<TProperty>(this DbFunctions _, TProperty property, string value) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Term)));

    public static bool Term<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Fuzzy fuzzy
    ) => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Term)));

    public static bool Term<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Boost boost
    ) => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Term)));

    public static bool Term<TProperty>(
        this DbFunctions _,
        TProperty property,
        string value,
        [NotParameterized] Fuzzy fuzzy,
        [NotParameterized] Boost boost
    ) => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Term)));

    public static float Score<TProperty>(this DbFunctions _, TProperty property) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Score)));

    public static string? Snippet<TProperty>(this DbFunctions _, TProperty property) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Snippet)));

    public static string? Snippet<TProperty>(
        this DbFunctions _,
        TProperty property,
        int maxNumChars
    ) => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Snippet)));

    public static string? Snippet<TProperty>(
        this DbFunctions _,
        TProperty property,
        string startTag,
        string endTag
    ) => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Snippet)));

    public static string? Snippet<TProperty>(
        this DbFunctions _,
        TProperty property,
        string startTag,
        string endTag,
        int maxNumChars
    ) => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Snippet)));

    // TODO: Multidimensional arrays are not yet supported: https://github.com/npgsql/efcore.pg/issues/314
    /*[DbFunction("snippet_positions", "paradedb", IsBuiltIn = false)]
    public static int[] SnippetPositions<TProperty>(
        this DbFunctions _,
        TProperty property
    )
    {
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(SnippetPositions)));
    }*/

    public static bool Proximity<TProperty>(
        this DbFunctions _,
        TProperty property,
        string token1,
        string token2,
        int maxDistance
    ) => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Proximity)));

    public static bool ProximityRegex<TProperty>(
        this DbFunctions _,
        TProperty property,
        [StringSyntax(StringSyntaxAttribute.Regex)] string pattern,
        string token,
        int maxDistance
    ) => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(ProximityRegex)));

    public static bool ProximityRegex<TProperty>(
        this DbFunctions _,
        TProperty property,
        [StringSyntax(StringSyntaxAttribute.Regex)] string pattern,
        string token,
        int maxDistance,
        int matchLimit
    ) => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(ProximityRegex)));

    public static IEnumerable<string> Tokenize<TProperty>(
        this DbFunctions _,
        TProperty property,
        [NotParameterized] Tokenizer tokenizer
    ) => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Tokenize)));
}
