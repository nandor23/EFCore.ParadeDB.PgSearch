using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.ParadeDB.PgSearch.Internal.Translators;

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

        return _sqlExpressionFactory.Function(
            name: "score",
            schema: "pdb",
            nullable: false,
            arguments: [arguments[1]],
            argumentsPropagateNullability: [false],
            returnType: typeof(float)
        );
    }
}
