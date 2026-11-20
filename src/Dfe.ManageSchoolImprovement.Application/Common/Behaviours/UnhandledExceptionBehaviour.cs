using System.Diagnostics.CodeAnalysis;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Dfe.ManageSchoolImprovement.Application.Common.Behaviours
{
    [ExcludeFromCodeCoverage]
    public class UnhandledExceptionBehaviour<TRequest, TResponse>(
        ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IMessage
    {
        public async ValueTask<TResponse> Handle(TRequest request, MessageHandlerDelegate<TRequest, TResponse> next,
            CancellationToken cancellationToken)
        {
#pragma warning disable S2139
            try
            {
                return await next(request, cancellationToken);
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                logger.LogError(ex, "PersonsAPI Request: Unhandled Exception for Request {Name} {@Request}",
                    requestName, request);

                throw;
            }
#pragma warning restore S2139
        }
    }
}