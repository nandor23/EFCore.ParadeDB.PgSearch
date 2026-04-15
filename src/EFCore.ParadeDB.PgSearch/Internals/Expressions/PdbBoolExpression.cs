using System.Collections.Frozen;
using EFCore.ParadeDB.PgSearch.Internals.TypeMappings;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace EFCore.ParadeDB.PgSearch.Internals.Expressions;

#pragma warning disable EF1001
internal sealed class PdbBoolExpression : PgUnknownBinaryExpression
{
    private static readonly FrozenDictionary<PdbOperatorType, string> OperatorMap = new Dictionary<
        PdbOperatorType,
        string
    >
    {
        { PdbOperatorType.Disjunction, "|||" },
        { PdbOperatorType.Conjunction, "&&&" },
        { PdbOperatorType.Phrase, "###" },
        { PdbOperatorType.Term, "===" },
        { PdbOperatorType.Function, "@@@" },
    }.ToFrozenDictionary();

    public PdbBoolExpression(SqlExpression left, SqlExpression right, PdbOperatorType operatorType)
        : base(left, right, OperatorMap[operatorType], typeof(bool), NpgsqlTypeMappings.Boolean) { }
}
