using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Dfe.ManageSchoolImprovement.Application.Common.Behaviours
{
    [ExcludeFromCodeCoverage]
    public class PerformanceBehaviour<TRequest, TResponse>(
        ILogger<PerformanceBehaviour<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IMessage
    {
        private readonly Stopwatch _timer = new();
        private readonly bool _performanceLoggingEnabled = configuration.GetValue<bool>("Features:PerformanceLoggingEnabled");

        public async ValueTask<TResponse> Handle(TRequest request, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
        {
            if (!_performanceLoggingEnabled)
            {
                return await next(request, cancellationToken);
            }

            _timer.Start();

            var response = await next(request, cancellationToken);

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds <= 500) return response;

            var user = httpContextAccessor.HttpContext?.User;

            var requestName = typeof(TRequest).Name;
            var identityName = user?.Identity?.Name;

            logger.LogWarning("PersonsAPI Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@IdentityName} {@Request}",
                requestName, elapsedMilliseconds, identityName, request);

            return response;
        }
    }

}
