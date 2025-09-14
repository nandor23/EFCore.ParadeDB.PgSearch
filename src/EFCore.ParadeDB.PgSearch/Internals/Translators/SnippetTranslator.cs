using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.ParadeDB.PgSearch.Internals.Translators;

internal sealed class SnippetTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public SnippetTranslator(ISqlExpressionFactory sqlExpressionFactory)
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
        if (method.Name != nameof(PgSearch.Snippet))
        {
            return null;
        }

        List<SqlExpression> args = [arguments[0]];
        List<bool> argsNullability = [false];

        // TODO handle other arguments

        return _sqlExpressionFactory.Function(
            name: "snippet",
            schema: "paradedb",
            nullable: false,
            arguments: args,
            argumentsPropagateNullability: argsNullability,
            returnType: typeof(string)
        );
    }
}
