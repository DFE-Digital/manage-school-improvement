using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Mediator;
using ValidationException = Dfe.ManageSchoolImprovement.Application.Common.Exceptions.ValidationException;

namespace Dfe.ManageSchoolImprovement.Application.Common.Behaviours;

[ExcludeFromCodeCoverage]
public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
    public async ValueTask<TResponse> Handle(TRequest request, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next(request, cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Count > 0)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count > 0)
            throw new ValidationException(failures);
        return await next(request, cancellationToken);
    }
}
