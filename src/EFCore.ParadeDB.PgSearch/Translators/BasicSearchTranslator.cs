using System.Collections.Frozen;
using System.Data;
using System.Reflection;
using EFCore.ParadeDB.PgSearch.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

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

        SqlExpression valueExpr = arguments[1];

        if (valueExpr is SqlConstantExpression { TypeMapping: null } constant)
        {
            valueExpr = _sqlExpressionFactory.Constant(constant.Value, typeof(string));
        }
        

        return new PgSearchExpression(arguments[0], valueExpr, op);
    }
}
