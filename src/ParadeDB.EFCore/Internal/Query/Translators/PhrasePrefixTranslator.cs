using System.Reflection;

using ParadeDB.EFCore.Extensions;
using ParadeDB.EFCore.Internal.Query.Expressions;
using ParadeDB.EFCore.Internal.Storage;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ParadeDB.EFCore.Internal.Query.Translators;

internal sealed class PhrasePrefixTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public PhrasePrefixTranslator(ISqlExpressionFactory sqlExpressionFactory)
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
        if (method.Name != nameof(ParadeDbFunctionsExtensions.PhrasePrefix))
        {
            return null;
        }

        var left = arguments[1];

        var args = new List<SqlExpression> { _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[2]) };
        var nullability = new List<bool> { false };

        for (var i = 3; i < arguments.Count; i++)
        {
            var arg = arguments[i];

            if (arg is SqlConstantExpression { Value: null })
            {
                continue;
            }

            args.Add(_sqlExpressionFactory.ApplyDefaultTypeMapping(arg));
            nullability.Add(false);
        }

        var phrasePrefix = _sqlExpressionFactory.Function(
            name: "phrase_prefix",
            schema: "pdb",
            nullable: false,
            arguments: args,
            argumentsPropagateNullability: nullability,
            returnType: typeof(bool),
            typeMapping: PdbTypeMappings.Boolean
        );

        return new PdbBoolExpression(left, phrasePrefix, PdbOperatorType.Function);
    }
}
