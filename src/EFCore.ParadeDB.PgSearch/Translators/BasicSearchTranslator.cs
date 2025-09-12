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
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    private static readonly FrozenDictionary<string, string> MethodOperatorMap = new Dictionary<
        string,
        string
    >
    {
        [nameof(PgSearch.MatchDisjunction)] = "|||",
        [nameof(PgSearch.MatchConjunction)] = "&&&",
        [nameof(PgSearch.Phrase)] = "###",
        [nameof(PgSearch.Term)] = "===",
    }.ToFrozenDictionary();

    public BasicSearchTranslator(ISqlExpressionFactory sqlExpressionFactory)
    {
        _sqlExpressionFactory = sqlExpressionFactory;
    }

    public SqlExpression? Translate(
        SqlExpression? instance,
        MethodInfo method,
        IReadOnlyList<SqlExpression> arguments,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger
    )
    {
        if (!MethodOperatorMap.TryGetValue(method.Name, out var op))
        {
            return null;
        }

        var left = _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[0]);
        var right = _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[1]);

        return new PgSearchExpression(left, right, op);
    }
}
