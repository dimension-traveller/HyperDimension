using FluentValidation;
using HyperDimension.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Application.Common.Behavior;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<IActionResult>
    where TResponse : IActionResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any() is false)
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(x => x.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(x => x.Errors.Count != 0)
            .SelectMany(x => x.Errors)
            .ToList();

        var failureMessages = failures.Select(x => x.ErrorMessage).ToArray();
        if (failureMessages.Length == 0)
        {
            return await next();
        }

        var errorMessage = string.Join("\n", failureMessages);
        return (TResponse)(IActionResult)new ErrorMessageResult(errorMessage).ToBadRequest();
    }
}
