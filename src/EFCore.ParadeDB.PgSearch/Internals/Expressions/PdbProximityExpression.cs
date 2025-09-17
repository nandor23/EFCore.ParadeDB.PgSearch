using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace EFCore.ParadeDB.PgSearch.Internals.Expressions;

#pragma warning disable EF1001
internal sealed class PdbProximityExpression : PgUnknownBinaryExpression
{
    private static readonly Type BoolType = typeof(string);
    private static readonly RelationalTypeMapping BoolTypeMapping = new BoolTypeMapping("boolean");

    public PdbProximityExpression(SqlExpression left, SqlExpression right)
        : base(left, right, "##", BoolType, BoolTypeMapping) { }
}
