using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ParadeDB.EFCore.Extensions;
using ParadeDB.EFCore.Internal.Query.Expressions;

namespace ParadeDB.EFCore.Internal.Query.Translators;

#pragma warning disable EF1001
internal sealed class ProximityTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public ProximityTranslator(ISqlExpressionFactory sqlExpressionFactory)
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
        if (method.Name != nameof(PgSearchDbFunctionsExtensions.Proximity))
        {
            return null;
        }

        if (arguments[5] is not SqlConstantExpression { Value: bool ordered })
        {
            return null;
        }

        var leftProximity = new PdbProximityExpression(
            _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[2]),
            _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[4]),
            ordered
        );

        var fullProximity = new PdbProximityExpression(
            leftProximity,
            _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[3]),
            ordered
        );

        return new PdbBoolExpression(arguments[1], fullProximity, PdbOperatorType.Function);
    }
}
