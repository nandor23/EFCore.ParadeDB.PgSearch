using EFCore.ParadeDB.PgSearch.Storage.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace EFCore.ParadeDB.PgSearch.Query.Internal.Expressions;

#pragma warning disable EF1001
internal sealed class PdbProximityExpression : PgUnknownBinaryExpression
{
    public PdbProximityExpression(SqlExpression left, SqlExpression right, bool ordered = false)
        : base(left, right, ordered ? "##>" : "##", typeof(bool), PdbTypeMappings.Boolean) { }
}
