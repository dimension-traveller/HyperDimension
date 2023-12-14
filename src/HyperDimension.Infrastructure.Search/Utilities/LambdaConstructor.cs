using System.Linq.Expressions;
using System.Reflection;
using HyperDimension.Application.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Search.Utilities;

public static class LambdaConstructor
{
    public static Expression<Func<TEntity, bool>> BuildStringExpression<TEntity>(
        string propertyName, MethodInfo method, params object[] methodArguments)
    {
        var parameter = Expression.Parameter(typeof(TEntity));
        var property = typeof(TEntity).GetProperty(propertyName).ExpectNotNull();

        var arguments = methodArguments.Select(Expression.Constant);

        var expression = Expression.Call(Expression.Property(parameter, property), method, arguments);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(expression, parameter);
        return lambda;
    }

    public static Expression<Func<TEntity, bool>> BuildPgroongaExpression<TEntity>(
        string propertyName, string methodName, params object[] methodArguments)
    {
        var parameter = Expression.Parameter(typeof(TEntity));
        var property = typeof(TEntity).GetProperty(propertyName).ExpectNotNull();
        var useArrayVariant = property.PropertyType != typeof(string);
        var methodInfo = GetPgroongaLinqMethod(methodName, useArrayVariant);

        Expression[] arguments = [
            Expression.Property(parameter, property),
            ..methodArguments.Select(Expression.Constant)
        ];

        var expression = Expression.Call(null, methodInfo, arguments);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(expression, parameter);
        return lambda;
    }

    private static readonly IEnumerable<MethodInfo> PgroongaStringLinqMethods = typeof(PGroongaLinqExtensions)
        .GetMethods()
        .Where(m => m
            .GetParameters()
            .First(p => p.Position == 1)
            .ParameterType
            .IsArray is false);

    private static readonly IEnumerable<MethodInfo> PgroongaStringArrayLinqMethods = typeof(PGroongaLinqExtensions)
        .GetMethods()
        .Where(m => m
            .GetParameters()
            .First(p => p.Position == 1)
            .ParameterType
            .IsArray);

    private static MethodInfo GetPgroongaLinqMethod(string name, bool arrayVariant = false)
    {
        return arrayVariant
            ? PgroongaStringArrayLinqMethods.First(x => x.Name == name)
            : PgroongaStringLinqMethods.First(x => x.Name == name);
    }
}
