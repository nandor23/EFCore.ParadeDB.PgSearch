using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Expressions;

internal sealed class PgSearchExpression : SqlExpression
{
    private readonly SqlExpression _left;
    private readonly SqlExpression _right;
    private readonly string _operator;
    private static readonly Type BoolType = typeof(bool);
    private static readonly RelationalTypeMapping BoolTypeMapping = new BoolTypeMapping("boolean");

    public PgSearchExpression(SqlExpression left, SqlExpression right, string op)
        : base(BoolType, BoolTypeMapping)
    {
        _left = left;
        _right = right;
        _operator = op;
    }

    public override Expression Quote()
    {
        return Expression.Quote(this);
    }

    protected override void Print(ExpressionPrinter expressionPrinter)
    {
        expressionPrinter.Visit(_left);
        expressionPrinter.Append($" {_operator} ");
        expressionPrinter.Visit(_right);
    }

    public override bool Equals(object? obj)
    {
        // TODO: Maybe the TypeMapping should be compared as well
        return obj is PgSearchExpression other
            && _left.Equals(other._left)
            && _right.Equals(other._right)
            && _operator == other._operator;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_left, _right, _operator);
    }
}
