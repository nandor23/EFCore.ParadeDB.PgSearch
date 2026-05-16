using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace ParadeDB.EFCore.Internal.Query;

internal sealed class ParadeDbEvaluatableExpressionFilterPlugin : IEvaluatableExpressionFilterPlugin
{
    public bool IsEvaluatableExpression(Expression expression)
    {
        if (expression is MethodCallExpression call && call.Method.ReturnType == typeof(PdbQuery))
        {
            return false;
        }

        return true;
    }
}
