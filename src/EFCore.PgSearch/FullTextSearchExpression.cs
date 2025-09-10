using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.PgSearch;

public sealed class FullTextSearchExpression : SqlExpression
{
    private readonly SqlExpression _left;
    private readonly SqlExpression _right;
    private static readonly Type BoolType = typeof(bool);
    private static readonly RelationalTypeMapping BoolTypeMapping = new BoolTypeMapping("boolean");

    public FullTextSearchExpression(SqlExpression left, SqlExpression right)
        : base(BoolType, BoolTypeMapping)
    {
        _left = left;
        _right = right;
    }

    public override Expression Quote()
    {
        return Expression.Quote(this);
    }

    protected override void Print(ExpressionPrinter expressionPrinter)
    {
        expressionPrinter.Visit(_left);
        expressionPrinter.Append(" @@@ ");
        expressionPrinter.Visit(_right);
    }

    public override bool Equals(object? obj)
    {
        // TODO: Maybe the TypeMapping should be compared as well
        return obj is FullTextSearchExpression other
            && _left.Equals(other._left)
            && _right.Equals(other._right);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_left, _right);
    }
}
