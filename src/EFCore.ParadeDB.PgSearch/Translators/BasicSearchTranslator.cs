using System.Collections.Frozen;
using System.Reflection;
using EFCore.ParadeDB.PgSearch.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.ParadeDB.PgSearch.Translators;

internal sealed class BasicSearchTranslator : IMethodCallTranslator
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
        if (!MethodOperatorMap.TryGetValue(method, out var op))
        {
            return null;
        }

        if (arguments.Count > 2 && arguments[2] is SqlConstantExpression { Value: Fuzzy fuzzy })
        {
            return new PgSearchExpression(
                arguments[0],
                new SqlFragmentExpression($"{arguments[1]}::{fuzzy.ToSql()}"),
                op
            );
        }

        return new PgSearchExpression(arguments[0], arguments[1], op);
    }
}
