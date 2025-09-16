using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.ParadeDB.PgSearch.Internals.Translators;

internal sealed class SnippetTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;
    private const string DefaultStartTag = "<b>";
    private const string DefaultEndTag = "</b>";
    private const int DefaultMaxNumChars = 150;

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
        if (method.Name != nameof(PgSearchFunctionsExtensions.Snippet))
        {
            return null;
        }

        List<SqlExpression> args = [arguments[1]];

        args.AddRange(
            arguments.Count switch
            {
                2 =>
                [
                    _sqlExpressionFactory.Constant(DefaultStartTag),
                    _sqlExpressionFactory.Constant(DefaultEndTag),
                    _sqlExpressionFactory.Constant(DefaultMaxNumChars),
                ],
                3 =>
                [
                    _sqlExpressionFactory.Constant(DefaultStartTag),
                    _sqlExpressionFactory.Constant(DefaultEndTag),
                    arguments[2],
                ],
                4 =>
                [
                    arguments[2],
                    arguments[3],
                    _sqlExpressionFactory.Constant(DefaultMaxNumChars),
                ],
                _ => [arguments[2], arguments[3], arguments[4]],
            }
        );

        return _sqlExpressionFactory.Function(
            name: "snippet",
            schema: "paradedb",
            nullable: true,
            arguments: args,
            argumentsPropagateNullability: [false, true, true, false],
            returnType: typeof(string)
        );
    }
}
