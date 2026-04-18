using System.Reflection;
using EFCore.ParadeDB.PgSearch.Extensions;
using EFCore.ParadeDB.PgSearch.Query.Internal.Expressions;
using EFCore.ParadeDB.PgSearch.Storage.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.ParadeDB.PgSearch.Query.Internal.Translators;

internal sealed class ProximityRegexTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public ProximityRegexTranslator(ISqlExpressionFactory sqlExpressionFactory)
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
        if (method.Name != nameof(PgSearchDbFunctionsExtensions.ProximityRegex))
        {
            return null;
        }

        List<SqlExpression> regexArgs = [arguments[2]];
        List<bool> regexArgsNullability = [false];

        if (arguments.Count > 5)
        {
            regexArgs.Add(arguments[5]);
            regexArgsNullability.Add(false);
        }

        var proxRegex = _sqlExpressionFactory.Function(
            name: "prox_regex",
            schema: "pdb",
            nullable: false,
            arguments: regexArgs,
            argumentsPropagateNullability: regexArgsNullability,
            returnType: typeof(bool),
            typeMapping: PdbTypeMappings.Boolean
        );

        var leftProximity = new PdbProximityExpression(
            proxRegex,
            _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[4])
        );

        var fullProximity = new PdbProximityExpression(
            leftProximity,
            _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[3])
        );

        return new PdbBoolExpression(arguments[1], fullProximity, PdbOperatorType.Function);
    }
}
