using EFCore.ParadeDB.PgSearch.Internals.TypeMappings;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace EFCore.ParadeDB.PgSearch.Internals.Expressions;

#pragma warning disable EF1001
internal sealed class PdbProximityExpression : PgUnknownBinaryExpression
{
    public PdbProximityExpression(SqlExpression left, SqlExpression right)
        : base(left, right, "##", typeof(bool), NpgsqlTypeMappings.Boolean) { }
}
