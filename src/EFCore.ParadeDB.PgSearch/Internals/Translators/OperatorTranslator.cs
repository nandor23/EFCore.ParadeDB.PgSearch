using System.Linq.Expressions;
using System.Reflection;
using EFCore.ParadeDB.PgSearch.Internals.Expressions;
using EFCore.ParadeDB.PgSearch.Internals.TypeMappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

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

        for (int i = 3; i < arguments.Count; i++)
        {
            right = ApplyModifier(right, arguments[i]) ?? right;
        }

        return new PdbBoolExpression(left, right, operatorType.Value);
    }

    private SqlExpression? ApplyModifier(SqlExpression expression, SqlExpression modifierExpression)
    {
        if (modifierExpression is not SqlConstantExpression { Value: var value })
        {
            return expression;
        }

        RelationalTypeMapping? typeMapping = value switch
        {
            Fuzzy fuzzy => new FuzzyTypeMapping(fuzzy),
            Boost boost => new BoostTypeMapping(boost),
            _ => null,
        };

        if (typeMapping is null)
        {
            return expression;
        }

        return _sqlExpressionFactory.MakeUnary(
            ExpressionType.Convert,
            expression,
            typeMapping.ClrType,
            typeMapping
        );
    }
}
