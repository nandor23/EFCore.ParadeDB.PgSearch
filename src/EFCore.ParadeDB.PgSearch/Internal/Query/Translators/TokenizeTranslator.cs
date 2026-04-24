using System.Linq.Expressions;
using System.Reflection;
using EFCore.ParadeDB.PgSearch.Extensions;
using EFCore.ParadeDB.PgSearch.Internal.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.ParadeDB.PgSearch.Internal.Query.Translators;

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
        bool? asArray = method.Name switch
        {
            // nameof(PgSearchFunctionsExtensions.Tokenize) => false,
            nameof(PgSearchDbFunctionsExtensions.TokenizeAsArray) => true,
            _ => null,
        };

        if (asArray is null)
        {
            return null;
        }

        if (arguments[2] is not SqlConstantExpression { Value: Tokenizer tokenizer })
        {
            return null;
        }

        var typeMapping = new TokenizerTypeMapping(tokenizer, asArray.Value);

        return _sqlExpressionFactory.MakeUnary(
            ExpressionType.Convert,
            arguments[1],
            typeMapping.ClrType,
            typeMapping
        );
    }
}
