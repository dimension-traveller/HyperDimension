using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace HyperDimension.Library.PGroonga.ExpressionTranslators;

public class PGroongaEvaluatableExpressionFilterPlugin : IEvaluatableExpressionFilterPlugin
{
    public bool IsEvaluatableExpression(Expression expression)
    {
        return !(expression is MethodCallExpression exp && exp.Method.DeclaringType == typeof(PGroongaDbFunctionsExtensions));
    }
}
