using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.ParadeDB.PgSearch.Internals.Translators;

internal sealed class ScoreTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public ScoreTranslator(ISqlExpressionFactory sqlExpressionFactory)
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
        if (method.Name != nameof(PgSearchFunctionsExtensions.Score))
        {
            return null;
        }

        var functionCall = _sqlExpressionFactory.Function(
            name: "score",
            schema: "paradedb",
            nullable: true,
            arguments: [arguments[1]],
            argumentsPropagateNullability: [false],
            returnType: typeof(float?)
        );

        return _sqlExpressionFactory.Coalesce(functionCall, _sqlExpressionFactory.Constant(0f));
    }
}
