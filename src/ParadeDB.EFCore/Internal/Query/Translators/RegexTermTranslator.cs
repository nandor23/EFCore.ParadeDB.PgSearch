using System.Linq.Expressions;
using System.Reflection;
using ParadeDB.EFCore.Extensions;
using ParadeDB.EFCore.Internal.Query.Expressions;
using ParadeDB.EFCore.Internal.Storage;
using ParadeDB.EFCore.Modifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ParadeDB.EFCore.Internal.Query.Translators;

internal sealed class RegexTermTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public RegexTermTranslator(ISqlExpressionFactory sqlExpressionFactory)
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
        if (method.Name != nameof(ParadeDbFunctionsExtensions.RegexTerm))
        {
            return null;
        }

        var left = arguments[1];
        var pattern = _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[2]);
        var regex = _sqlExpressionFactory.Function(
            name: "regex",
            schema: "pdb",
            nullable: false,
            arguments: [pattern],
            argumentsPropagateNullability: [false],
            returnType: typeof(bool),
            typeMapping: PdbTypeMappings.Boolean
        );

        SqlExpression right = regex;

        if (arguments.Count == 4 && arguments[3] is SqlConstantExpression { Value: Boost boost })
        {
            right = _sqlExpressionFactory.MakeUnary(
                ExpressionType.Convert,
                regex,
                typeof(Boost),
                new BoostTypeMapping(boost)
            )!;
        }

        return new PdbBoolExpression(left, right, PdbOperatorType.Function);
    }
}
