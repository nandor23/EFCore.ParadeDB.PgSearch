using System.Collections.Frozen;
using System.Linq.Expressions;
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

    private static readonly FrozenDictionary<string, PdbOperatorType> MethodOperatorMap =
        new Dictionary<string, PdbOperatorType>
        {
            [nameof(PgSearch.MatchDisjunction)] = PdbOperatorType.Disjunction,
            [nameof(PgSearch.MatchConjunction)] = PdbOperatorType.Conjunction,
            [nameof(PgSearch.Phrase)] = PdbOperatorType.Phrase,
            [nameof(PgSearch.Term)] = PdbOperatorType.Term,
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
        if (!MethodOperatorMap.TryGetValue(method.Name, out var operatorType))
        {
            return null;
        }

        var left = arguments[0];
        var right = _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[1]);

        if (arguments.Count > 2 && arguments[2] is SqlConstantExpression { Value: Fuzzy fuzzy })
        {
            right = new SqlUnaryExpression(
                ExpressionType.Convert,
                right,
                typeof(string),
                new FuzzyTypeMapping(fuzzy)
            );
        }

        return new PdbBoolExpression(left, right, operatorType);
    }
}
