using System.Linq.Expressions;
using System.Reflection;
using EFCore.ParadeDB.PgSearch.Internal.TypeMappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.ParadeDB.PgSearch.Internal.Translators;

internal sealed class AliasTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public AliasTranslator(ISqlExpressionFactory sqlExpressionFactory)
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
        if (method.Name != nameof(PgSearchFunctionsExtensions.Alias))
        {
            return null;
        }

        if (arguments[2] is not SqlConstantExpression { Value: string alias })
        {
            return null;
        }

        var typeMapping = new AliasTypeMapping(alias);

        return _sqlExpressionFactory.MakeUnary(
            ExpressionType.Convert,
            arguments[1],
            typeMapping.ClrType,
            typeMapping
        );
    }
}
