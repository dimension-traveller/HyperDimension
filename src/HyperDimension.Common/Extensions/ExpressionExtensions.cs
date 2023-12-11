using System.Linq.Expressions;
using System.Reflection;

namespace HyperDimension.Common.Extensions;

// Modified from Entity Framework Core (EF Core) source code
// repo: https://github.com/dotnet/efcore
// commit: 5c670bba0e4a3922de243924e923ef9fdc0a9f3f
// tag: v8.0.0
// at: src/EFCore/Extensions/Internal/ExpressionExtensions.cs
// at: src/EFCore/Infrastructure/ExpressionExtensions.cs
public static class ExpressionExtensions
{
    public static PropertyInfo? GetPropertyInfo(this LambdaExpression expression)
    {
        return expression.GetMemberInfo<PropertyInfo>();
    }

    public static MemberInfo? GetMemberInfo(this LambdaExpression expression)
    {
        return expression.GetMemberInfo<MemberInfo>();
    }

    public static TMemberInfo? GetMemberInfo<TMemberInfo>(this LambdaExpression expression) where TMemberInfo : MemberInfo
    {
        var parameter = expression.Parameters[0];

        var memberInfos = new List<TMemberInfo>();
        var unwrappedExpression = RemoveTypeAs(RemoveConvert(expression.Body));
        do
        {
            var memberExpression = unwrappedExpression as MemberExpression;

            if (memberExpression?.Member is not TMemberInfo memberInfo)
            {
                return null;
            }

            memberInfos.Insert(0, memberInfo);

            unwrappedExpression = RemoveTypeAs(RemoveConvert(memberExpression.Expression));
        }
        while (unwrappedExpression != parameter);

        return memberInfos.Count == 1 ? memberInfos[0] : null;
    }

    private static Expression? RemoveTypeAs(this Expression? expression)
    {
        while (expression?.NodeType is ExpressionType.TypeAs)
        {
            expression = (RemoveConvert(expression) as UnaryExpression)?.Operand;
        }

        return expression;
    }

    private static Expression? RemoveConvert(this Expression? expression)
    {
        while (true)
        {
            if (expression is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unaryExpression)
            {
                expression = unaryExpression.Operand;
                continue;
            }

            return expression;
        }
    }
}
