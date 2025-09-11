using System.Collections.Frozen;
using System.Reflection;
using EFCore.PgSearch.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.PgSearch.Translators;

internal class BasicSearchTranslator : IMethodCallTranslator
{
    private static readonly FrozenDictionary<MethodInfo, string> MethodOperatorMap = new Dictionary<
        MethodInfo,
        string
    >
    {
        [typeof(PgSearch).GetMethod(nameof(PgSearch.MatchDisjunction))!] = "|||",
        [typeof(PgSearch).GetMethod(nameof(PgSearch.MatchConjunction))!] = "&&&",
        [typeof(PgSearch).GetMethod(nameof(PgSearch.Phrase))!] = "###",
        [typeof(PgSearch).GetMethod(nameof(PgSearch.Term))!] = "===",
    }.ToFrozenDictionary();

    public SqlExpression? Translate(
        SqlExpression? instance,
        MethodInfo method,
        IReadOnlyList<SqlExpression> arguments,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger
    )
    {
        return !MethodOperatorMap.TryGetValue(method, out var op)
            ? null
            : new PgSearchExpression(arguments[0], arguments[1], op);
    }
}
