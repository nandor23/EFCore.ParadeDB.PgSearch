using System.Linq.Expressions;
using System.Reflection;

using EFCore.ParadeDB.PgSearch.Internal.TypeMappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.ParadeDB.PgSearch.Internal.Translators;

internal sealed class TokenizeTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public TokenizeTranslator(ISqlExpressionFactory sqlExpressionFactory)
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
        if (method.Name != nameof(PgSearchFunctionsExtensions.Tokenize))
        {
            return null;
        }

        if (arguments[2] is not SqlConstantExpression { Value: Tokenizer tokenizer })
        {
            return null;
        }

        var typeMapping = new TokenizerTypeMapping(tokenizer);

        return _sqlExpressionFactory.MakeUnary(
            ExpressionType.Convert,
            arguments[1],
            typeMapping.ClrType,
            typeMapping
        );
    }
}
