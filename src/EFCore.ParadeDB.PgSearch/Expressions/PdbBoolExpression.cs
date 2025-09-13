using System.Collections.Frozen;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace EFCore.ParadeDB.PgSearch.Expressions;

#pragma warning disable EF1001
internal sealed class PdbBoolExpression : PgUnknownBinaryExpression
{
    private static readonly Type BoolType = typeof(bool);
    private static readonly RelationalTypeMapping BoolTypeMapping = new BoolTypeMapping("boolean");
    private static readonly FrozenDictionary<PdbOperatorType, string> OperatorMap = new Dictionary<
        PdbOperatorType,
        string
    >
    {
        { PdbOperatorType.Disjunction, "|||" },
        { PdbOperatorType.Conjunction, "&&&" },
        { PdbOperatorType.Phrase, "###" },
        { PdbOperatorType.Term, "===" },
    }.ToFrozenDictionary();

    public PdbBoolExpression(SqlExpression left, SqlExpression right, PdbOperatorType operatorType)
        : base(left, right, OperatorMap[operatorType], BoolType, BoolTypeMapping) { }
}
