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

internal sealed class RegexPhraseTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public RegexPhraseTranslator(ISqlExpressionFactory sqlExpressionFactory)
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
        if (method.Name != nameof(ParadeDbFunctionsExtensions.RegexPhrase))
        {
            return null;
        }

        var left = arguments[1];
        Boost? boost = null;

        var args = new List<SqlExpression>();
        var nullability = new List<bool>();

        args.Add(_sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[2]));
        nullability.Add(false);

        for (var i = 3; i < arguments.Count; i++)
        {
            var arg = arguments[i];

            if (arg is SqlConstantExpression { Value: Boost boostValue })
            {
                boost = boostValue;
                continue;
            }

            if (arg is SqlConstantExpression { Value: null })
            {
                continue;
            }

            args.Add(_sqlExpressionFactory.ApplyDefaultTypeMapping(arg));
            nullability.Add(false);
        }

        var regexPhrase = _sqlExpressionFactory.Function(
            name: "regex_phrase",
            schema: "pdb",
            nullable: false,
            arguments: args,
            argumentsPropagateNullability: nullability,
            returnType: typeof(bool),
            typeMapping: PdbTypeMappings.Boolean
        );

        SqlExpression right = regexPhrase;

        if (boost is not null)
        {
            right = _sqlExpressionFactory.MakeUnary(
                ExpressionType.Convert,
                regexPhrase,
                typeof(Boost),
                new BoostTypeMapping(boost.Value)
            )!;
        }

        return new PdbBoolExpression(left, right, PdbOperatorType.Function);
    }
}
