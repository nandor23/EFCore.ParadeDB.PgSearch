using System.Reflection;
using EFCore.ParadeDB.PgSearch.Internals.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internals.Translators;

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
        if (method.Name != nameof(PgSearchFunctionsExtensions.ProximityRegex))
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
            typeMapping: new BoolTypeMapping("boolean")
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
