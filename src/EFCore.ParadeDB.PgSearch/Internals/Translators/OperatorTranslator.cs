using System.Linq.Expressions;
using System.Reflection;
using EFCore.ParadeDB.PgSearch.Internals.Expressions;
using EFCore.ParadeDB.PgSearch.Internals.TypeMappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.ParadeDB.PgSearch.Internals.Translators;

internal sealed class OperatorTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public OperatorTranslator(ISqlExpressionFactory sqlExpressionFactory)
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
        PdbOperatorType? operatorType = method.Name switch
        {
            nameof(PgSearchFunctionsExtensions.MatchDisjunction) => PdbOperatorType.Disjunction,
            nameof(PgSearchFunctionsExtensions.MatchConjunction) => PdbOperatorType.Conjunction,
            nameof(PgSearchFunctionsExtensions.Phrase) => PdbOperatorType.Phrase,
            nameof(PgSearchFunctionsExtensions.Term) => PdbOperatorType.Term,
            _ => null,
        };

        if (operatorType is null)
        {
            return null;
        }

        var left = arguments[1];
        var right = _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[2]);

        if (arguments.Count > 3 && arguments[3] is SqlConstantExpression { Value: Fuzzy fuzzy })
        {
            right = new SqlUnaryExpression(
                ExpressionType.Convert,
                right,
                typeof(string),
                new FuzzyTypeMapping(fuzzy)
            );
        }

        return new PdbBoolExpression(left, right, operatorType.Value);
    }
}
