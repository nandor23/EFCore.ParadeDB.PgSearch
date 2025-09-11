using System.Reflection;
using EFCore.PgSearch.Tokenizers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.PgSearch;

public sealed class MatchTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    private static readonly MethodInfo MatchMethod = typeof(PgSearch).GetMethod(
        nameof(PgSearch.Match)
    )!;

    public MatchTranslator(ISqlExpressionFactory sqlExpressionFactory)
    {
        _sqlExpressionFactory = sqlExpressionFactory;
    }

    public SqlExpression? Translate(
        SqlExpression? _,
        MethodInfo method,
        IReadOnlyList<SqlExpression> arguments,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger
    )
    {
        if (!method.Equals(MatchMethod))
        {
            return null;
        }

        var args = new List<SqlExpression> { arguments[1], arguments[2] };
        var argsNullability = new List<bool> { true, true };

        if (arguments[3] is SqlConstantExpression { Value: Tokenizer tokenizer })
        {
            args.Add(tokenizer.ToSqlExpression());
            argsNullability.Add(true);
        }

        var matchFunction = _sqlExpressionFactory.Function(
            "paradedb.match",
            args,
            nullable: true,
            argumentsPropagateNullability: argsNullability,
            typeof(string)
        );

        return new FullTextSearchExpression(arguments[0], matchFunction);
    }
}
