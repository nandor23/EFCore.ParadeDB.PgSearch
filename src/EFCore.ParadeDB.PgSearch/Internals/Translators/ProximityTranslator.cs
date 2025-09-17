using System.Reflection;
using EFCore.ParadeDB.PgSearch.Internals.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.ParadeDB.PgSearch.Internals.Translators;

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
        if (method.Name != nameof(PgSearchFunctionsExtensions.Proximity))
        {
            return null;
        }

        var leftProximity = new PdbProximityExpression(
            _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[2]),
            _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[4])
        );

        var fullProximity = new PdbProximityExpression(
            leftProximity,
            _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[3])
        );

        return new PdbBoolExpression(arguments[1], fullProximity, PdbOperatorType.Function);
    }
}
